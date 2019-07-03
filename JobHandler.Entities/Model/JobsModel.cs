namespace JobHandler.Entities.Model
{
    public class JobsModel
    {
        public string Name { get; private set; }
        public string[] Dependencies { get; private set; }

        public JobsModel(string name, params string[] dependencies)
        {
            Name = name;
            Dependencies = dependencies;
        }
    }
}
