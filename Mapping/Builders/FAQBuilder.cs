using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.F.A.Q.Domain;

namespace Nop.Plugin.F.A.Q.Mapping.Builders;
public class FAQBuilder : NopEntityBuilder<FAQEntity>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
       table.WithColumn(nameof(FAQEntity.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(FAQEntity.Question)).AsString().NotNullable()
            .WithColumn(nameof(FAQEntity.ProudctName)).AsString().NotNullable() 
            .WithColumn(nameof(FAQEntity.Answer)).AsString().Nullable()
            .WithColumn(nameof(FAQEntity.Upvotes)).AsInt32().Nullable()
            .WithColumn(nameof(FAQEntity.ProductId)).AsInt32().NotNullable()
            .WithColumn(nameof(FAQEntity.Visibility)).AsByte().NotNullable()
            .WithColumn(nameof(FAQEntity.AskedDate)).AsDateTime().NotNullable()
            .WithColumn(nameof(FAQEntity.LastModified)).AsDateTime().Nullable();
            
    }
}
