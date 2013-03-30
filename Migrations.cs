using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Contrib.Widgets {
    [OrchardFeature("Contrib.Widgets")]
    public class Migrations : DataMigrationImpl {
        public int Create() {

            SchemaBuilder.CreateTable("WidgetExPartRecord", table => table
                .ContentPartRecord()
                .Column<int>("HostId"));

            ContentDefinitionManager.AlterPartDefinition("WidgetExPart", part => part.Attachable(false));

            ContentDefinitionManager.AlterPartDefinition("WidgetsContainerPart", part => part
                .Attachable()
                .WithDescription("Enables content items to contain widgets, removing the need to create a layer rule per content item."));

            return 1;
        }
    }
}