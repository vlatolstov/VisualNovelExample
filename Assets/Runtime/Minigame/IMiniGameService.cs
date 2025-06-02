using Naninovel;

using System.Threading;
public interface IMiniGameService : IEngineService {
    UniTask PlayAsync(CancellationToken cancellationToken = default);
    bool WasLastGameSuccessful { get; }
}