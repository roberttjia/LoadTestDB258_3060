#!/usr/bin/env python3
"""
Test EF Core's true limits with simple entities (no complex relationships)
This will help us understand if 258 is EF Core's limit or just our pattern's limit
"""

def generate_simple_entities(count):
    """Generate simple entities with minimal relationships"""
    entities = []
    
    for i in range(1, count + 1):
        entity_name = f"SimpleEntity{i:03d}"
        
        content = f"""using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{{
    public partial class {entity_name}
    {{
        public int Id {{ get; set; }}
        public string Name {{ get; set; }} = string.Empty;
        public string Description {{ get; set; }} = string.Empty;
        public decimal Amount {{ get; set; }}
        public DateTime CreatedDate {{ get; set; }}
        public int CategoryId {{ get; set; }}
        public string Status {{ get; set; }} = string.Empty;
        public bool IsActive {{ get; set; }}
    }}
}}"""
        
        with open(f"LoadTest.Data/Models/{entity_name}.cs", 'w') as f:
            f.write(content)
        
        entities.append(entity_name)
        
        if i % 50 == 0:
            print(f"Generated {i} simple entities...")
    
    return entities

def update_dbcontext_with_simple_entities(entities):
    """Update ApplicationDbContext with simple entities only"""
    
    # Read existing context
    with open("LoadTest.Data/ApplicationDbContext.cs", 'r') as f:
        content = f.read()
    
    # Find where DbSets start and end
    start_marker = "public class ApplicationDbContext : DbContext"
    end_marker = "protected override void OnModelCreating"
    
    start_idx = content.find(start_marker)
    end_idx = content.find(end_marker)
    
    if start_idx == -1 or end_idx == -1:
        print("Could not find DbSet section in ApplicationDbContext")
        return
    
    # Generate simple DbSet declarations
    dbset_declarations = []
    for entity in entities:
        dbset_declarations.append(f"        public virtual DbSet<{entity}> {entity} {{ get; set; }}")
    
    dbsets_content = "\n".join(dbset_declarations)
    
    # Create new context content
    new_content = f"""using Microsoft.EntityFrameworkCore;
using LoadTest.Data.Models;

namespace LoadTest.Data
{{
    public class ApplicationDbContext : DbContext
    {{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {{
        }}

{dbsets_content}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {{
            base.OnModelCreating(modelBuilder);
            
            // Simple entities - no complex relationships
            // This tests EF Core's raw entity handling capacity
        }}
    }}
}}"""
    
    with open("LoadTest.Data/ApplicationDbContext.cs", 'w') as f:
        f.write(new_content)
    
    print(f"Updated ApplicationDbContext with {len(entities)} simple entities")

def main():
    print("🧪 Testing EF Core's True Limits with Simple Entities")
    print("=" * 60)
    
    # Test with 500 simple entities
    print("Generating 500 simple entities (no complex relationships)...")
    entities = generate_simple_entities(500)
    
    print(f"Generated {len(entities)} simple entities")
    
    # Update ApplicationDbContext
    update_dbcontext_with_simple_entities(entities)
    
    print("\n" + "=" * 60)
    print("🎯 Simple Entity Test Ready!")
    print("Next steps:")
    print("1. dotnet build LoadTest.Data")
    print("2. Check if EF Core can handle 500 simple entities")
    print("3. If successful, this proves our 258 limit was due to complexity, not EF Core limits")

if __name__ == "__main__":
    main()