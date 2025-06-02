using Naninovel;

[CommandAlias("playMiniGame")]
public class PlayMiniGameCommand : Command {
    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default) {
        var service = Engine.GetService<IMiniGameService>();
        await service.PlayAsync(asyncToken.CancellationToken);
    }
}