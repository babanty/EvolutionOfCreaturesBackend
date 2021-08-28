namespace Infrastructure.Tools.Db
{
    public interface ICanBeEditedOffline
    {
        /// <summary> Всякий раз при изменении версия инкриментируется </summary>
        int Version { get; set; }
    }
}
