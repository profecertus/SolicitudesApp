using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TienditaInfra.Persistencia;

public partial class TienditaDBContext : DbContext
{
    public TienditaDBContext()
    {
    }

    public TienditaDBContext(DbContextOptions<TienditaDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=TienditaDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "UQ_Categorias_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true, "DF_Categorias_Activo");
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.DetalleId);

            entity.HasIndex(e => e.ProductoId, "IX_DetalleVentas_ProductoId");

            entity.HasIndex(e => e.VentaId, "IX_DetalleVentas_VentaId");

            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentas_Productos");

            entity.HasOne(d => d.Venta).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("FK_DetalleVentas_Ventas");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasIndex(e => e.CategoriaId, "IX_Productos_CategoriaId");

            entity.HasIndex(e => e.ProveedorId, "IX_Productos_ProveedorId");

            entity.HasIndex(e => e.CodigoBarras, "UQ_Productos_CodigoBarras").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true, "DF_Productos_Activo");
            entity.Property(e => e.CodigoBarras).HasMaxLength(30);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())", "DF_Productos_FechaRegistro");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PrecioCompra).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioVenta).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Categorias");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Productos)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Proveedores");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.ProveedorId);

            entity.HasIndex(e => e.Ruc, "UQ_Proveedores_Ruc").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true, "DF_Proveedores_Activo");
            entity.Property(e => e.Direccion).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.RazonSocial).HasMaxLength(100);
            entity.Property(e => e.Ruc).HasMaxLength(11);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasIndex(e => e.FechaVenta, "IX_Ventas_FechaVenta").IsDescending();

            entity.HasIndex(e => e.NumeroBoleta, "UQ_Ventas_NumeroBoleta").IsUnique();

            entity.Property(e => e.ClienteNombre)
                .HasMaxLength(100)
                .HasDefaultValue("Público General", "DF_Ventas_ClienteNombre");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Completada", "DF_Ventas_Estado");
            entity.Property(e => e.FechaVenta).HasDefaultValueSql("(getdate())", "DF_Ventas_FechaVenta");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(30)
                .HasDefaultValue("Efectivo", "DF_Ventas_MetodoPago");
            entity.Property(e => e.NumeroBoleta).HasMaxLength(20);
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
