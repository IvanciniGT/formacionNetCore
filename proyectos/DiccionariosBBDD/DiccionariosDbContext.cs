using Microsoft.EntityFrameworkCore;
using DiccionariosBBDD.Entities;

namespace DiccionariosBBDD;

public class DiccionariosDbContext : DbContext
{
    public DbSet<IdiomaEntity> Idiomas { get; set; }
    public DbSet<DiccionarioEntity> Diccionarios { get; set; }
    public DbSet<PalabraEntity> Palabras { get; set; }
    public DbSet<SignificadoEntity> Significados { get; set; }

    public DiccionariosDbContext(DbContextOptions<DiccionariosDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar relaciones
        modelBuilder.Entity<DiccionarioEntity>()
            .HasOne(d => d.Idioma)
            .WithMany(i => i.Diccionarios)
            .HasForeignKey(d => d.IdiomaId);

        // En el SQL Se traduce en:
        // ALTER TABLE Diccionarios ADD CONSTRAINT FK_Diccionarios_Idiomas_IdiomaId FOREIGN KEY (IdiomaId) REFERENCES Idiomas(Id) ON DELETE CASCADE;

        modelBuilder.Entity<PalabraEntity>()
            .HasOne(p => p.Diccionario)
            .WithMany(d => d.Palabras)
            .HasForeignKey(p => p.DiccionarioId);

        modelBuilder.Entity<SignificadoEntity>()
            .HasOne(s => s.Palabra)
            .WithMany(p => p.Significados)
            .HasForeignKey(s => s.PalabraId);

        // Configurar índices para optimización
        modelBuilder.Entity<IdiomaEntity>()
            .HasIndex(i => i.Codigo)
            .IsUnique();

        // TextoNormalizado eliminado - usamos índice funcional UPPER(Texto) en su lugar

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Crea índices funcionales para búsquedas case-insensitive eficientes.
    /// Se llama después de EnsureCreated() para aplicar los índices funcionales.
    /// </summary>
    public void CreateFunctionalIndexes()
    {
        try
        {
            // Índice funcional para búsquedas case-insensitive en código de idioma
            Database.ExecuteSqlRaw("CREATE INDEX IF NOT EXISTS IX_Idiomas_Codigo_Upper ON Idiomas (UPPER(Codigo))");
            
            // Índice funcional para búsquedas case-insensitive en texto de palabra
            Database.ExecuteSqlRaw("CREATE INDEX IF NOT EXISTS IX_Palabras_Texto_Upper ON Palabras (UPPER(Texto))");
        }
        catch (Exception ex)
        {
            // Los índices funcionales pueden no estar soportados en algunas versiones de SQLite
            // En producción se usaría el proveedor específico (PostgreSQL, SQL Server, etc.)
            System.Diagnostics.Debug.WriteLine($"Advertencia: No se pudieron crear índices funcionales: {ex.Message}");
        }
    }
}