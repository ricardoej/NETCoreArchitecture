using entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace data.map
{
    public class UsuarioMap: IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario", schema: "portal_seguranca");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(t => t.Nome).HasColumnName("nome");
            builder.Property(t => t.Login).HasColumnName("login").IsRequired();
            builder.Property(t => t.Email).HasColumnName("email").IsRequired();
            builder.Property(t => t.SenhaHash).HasColumnName("senha_hash").IsRequired();
            builder.Property(t => t.SenhaSalt).HasColumnName("senha_salt").IsRequired();
        }
    }
}