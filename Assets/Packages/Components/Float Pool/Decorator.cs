namespace FloatPool.Internal
{
    public interface IDecorator
    {
        void SetDecorable(IFloatPool decorable);
    }

    public abstract class Decorator : IFloatPool, IDecorator
    {
        private IFloatPool decorable;

        public virtual float Max => decorable.Max;
        public virtual float Current => decorable.Current;
        public virtual float Ratio => decorable.Ratio;

        public virtual (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false) => decorable.Decrease(amount, allowUnderflow);
        public virtual (float remaining, float taken) Increase(float amount, bool allowOverflow = false) => decorable.Increase(amount, allowOverflow);
        public virtual void Initialize() => decorable.Initialize();
        public virtual void UpdateBehaviour(float deltaTime) => decorable.UpdateBehaviour(deltaTime);

        void IDecorator.SetDecorable(IFloatPool decorable) => this.decorable = decorable;
    }
}
