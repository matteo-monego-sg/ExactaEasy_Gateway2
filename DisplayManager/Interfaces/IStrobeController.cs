using ExactaEasyCore;

namespace DisplayManager
{
    public interface IStrobeController 
    {
        int Id { get; }
        ParameterCollection<Parameter> GetStrobeParameter(int channelId);
        void SetStrobeParameter(int channelId, ParameterCollection<Parameter> parameters);
        void ApplyParameters(int channelId);
        void Dispose();
    }
}
