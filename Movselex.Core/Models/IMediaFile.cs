namespace Movselex.Core.Models
{
    public interface IMediaFile
    {
        long Id { get; }
        string FilePath { get; }
        string Length { get; }
        string VideoSize { get; }
        string Codec { get; }
    }
}