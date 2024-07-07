namespace HarvestFestival.States
{
    public class StateMachine<T>
    {
        protected T _currentState;

        public virtual void SetState(T state) => _currentState = state;
        public virtual T GetState() => _currentState;
    }
}