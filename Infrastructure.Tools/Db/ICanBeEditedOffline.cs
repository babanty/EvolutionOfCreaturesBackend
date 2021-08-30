namespace Infrastructure.Tools.Db
{
    public interface ICanBeEditedOffline
    {
        /// <summary> When class data changes, the field is incremented </summary>
        int Version { get; set; }
    }
}
