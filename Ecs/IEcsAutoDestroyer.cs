namespace BlitzEcs {
    public interface IEcsAutoDestroyer<TComponent> where TComponent : struct {
        void OnDestroy(ref TComponent component);
    }
}