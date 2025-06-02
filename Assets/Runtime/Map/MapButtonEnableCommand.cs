using System.Linq;

using Naninovel;
using Naninovel.Commands;

using UnityEngine;

[CommandAlias("mapButtonEnable")]
public class MapButtonEnableCommand : Command {
    [ParameterAlias("id"), RequiredParameter]
    public StringParameter Id;

    [ParameterAlias("enabled")]
    public BooleanParameter Enabled = true;

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default) {
        var button = Object.FindObjectsByType<MapLocationButton>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .FirstOrDefault(b => b.Id == Id);

        if (button == null) {
            Debug.LogWarning($"[MapButtonEnable] '{Id}' id not found.");
            return UniTask.CompletedTask;
        }

        button.SetEnabled(Enabled);
        return UniTask.CompletedTask;
    }
}
