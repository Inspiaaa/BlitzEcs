namespace BlitzEcs {
    public interface IEcsDestroyHandler<TComponent> where TComponent : struct {
        void OnDestroy(ref TComponent component);
    }
}