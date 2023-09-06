using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebGLives.Core;
using WebGLives.DataAccess.Entities;

namespace WebGLives.DataAccess.Configurations;

public class GameEntityConfiguration : IEntityTypeConfiguration<GameEntity>
{
    public void Configure(EntityTypeBuilder<GameEntity> builder)
    {
        builder.Property(game => game.Title).HasMaxLength(Game.MaxTitleLength);
        builder.Property(game => game.Description).HasMaxLength(Game.MaxDescriptionLength);
    }
}