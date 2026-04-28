namespace Logy.UnityCommonV01
{
    public class TestObjectPool
    {
        private ObjectPool<Test> _objectPool;
        public int idleCount => _objectPool.idleCount;
        public int usingCount => _objectPool.usingCount;

        public TestObjectPool(ushort _startAmount)
        {
            _objectPool = new(CreateObject, ReleaseObject, DestoryObject, _startAmount);
        }

        private Test CreateObject()
        {
            Test _object = new();
            _object.isCreate = true;

            return _object;
        }

        private void ReleaseObject(Test _object)
        {
            _object.isUse = false;
        }

        private void DestoryObject(Test _object)
        {
            _object.isDestory = true;
        }

        public Test Get() => _objectPool.Get();

        public void Release(Test _object) => _objectPool.Release(_object);

        public void ReleaseAll() => _objectPool.ReleaseAll();

        public void Destory() => _objectPool.Destory();
    }
}


