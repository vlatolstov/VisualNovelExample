using System.Threading;

using Naninovel;
public interface IMiniGameService : IEngineService {
    bool WasLastGameSuccessful { get; }
    UniTask PlayAsync(CancellationToken cancellationToken = default);
}
