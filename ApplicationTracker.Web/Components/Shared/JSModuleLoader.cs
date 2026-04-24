using Microsoft.JSInterop;

namespace ApplicationTracker.Web.Components.Shared
{
    public class JSModuleLoader(IJSRuntime _jsRuntime) : IJSModuleLoader, IAsyncDisposable
    {
        private readonly string[] _moduleNames = new[] { "browserInterop", "clipboardModule", "fileInputInteropModule", "pdfModule" };
        private readonly Dictionary<int, IJSObjectReference> _modules = new();

        public async Task RegisterAsync(params ModuleType[] modules)
        {
            if (modules == null || modules.Length == 0)
                return;

            foreach (var module in modules)
            {
                var idx = (int)module;
                if (_modules.ContainsKey(idx))
                    continue;

                var jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./js/{_moduleNames[idx]}.js");
                _modules[idx] = jsModule;
            }
        }

        public ValueTask<T> InvokeAsync<T>(ModuleType module, string functionName, params object[] args)
        {
            if (!_modules.ContainsKey((int)module))
                throw new InvalidOperationException($"Module {module} has not been registered for this instance.");

            return _modules[(int)module].InvokeAsync<T>(functionName, args);
        }

        public async Task ReleaseAsync()
        {
            foreach (var jsRef in _modules.Values)
            {
                try
                {
                    await jsRef.DisposeAsync();
                }
                catch
                {
                    // Swallow individual dispose errors to ensure best-effort cleanup
                }
            }

            _modules.Clear();
        }

        public async ValueTask DisposeAsync()
        {
            await ReleaseAsync();
        }
    }
}
