using NUnit.Framework;

namespace Logy.UnityCommonV01
{
    public class ObjectPoolUnitTest
    {
        private TestObjectPool _objectPool;

        // 檢查建立10個物件，idleCount為10，usingCount為0
        [Test]
        public void CheckCreate10()
        {
            _objectPool = new(10);

            Assert.AreEqual(10, _objectPool.idleCount);
            Assert.AreEqual(0, _objectPool.usingCount);
            Assert.IsTrue(_objectPool.Get().isCreate);
        }

        // 檢查idleQueue.Count為1，取得10個物件，過程中idleQueue.Count為0時，會自動建立物件，最後idleQueue.Count為0，usingCount為10
        [Test]
        public void CheckGet10()
        {
            _objectPool = new(1);

            byte _count = 0;
            while (_count < 10)
            {
                _count++;
                var _object = _objectPool.Get();

                Assert.IsNotNull(_object);
                Assert.AreEqual(0, _objectPool.idleCount);
            }

            Assert.AreEqual(10, _objectPool.usingCount);
        }

        // 檢查idleCount為0，建立10個物件，建立馬上釋放，最後idleCount為10，usingCount為0
        [Test]
        public void CheckRelease10()
        {
            _objectPool = new(0);

            byte _count = 0;
            while (_count < 10)
            {
                _count++;
                var _object = _objectPool.Get();
                _object.isUse = true;
                _objectPool.Release(_object);

                Assert.AreEqual(0, _objectPool.usingCount);
                Assert.AreEqual(false, _object.isUse);
            }

            Assert.AreEqual(1, _objectPool.idleCount);
        }

        // 檢查釋放所有物件，idleCount為10，usingCount為0
        [Test]
        public void CheckReleaseAll()
        {
            _objectPool = new(0);

            byte _count = 0;
            while (_count < 10)
            {
                _count++;
                _objectPool.Get();
            }

            _objectPool.ReleaseAll();

            Assert.AreEqual(10, _objectPool.idleCount);
            Assert.AreEqual(0, _objectPool.usingCount);
        }

        // 檢查銷毀所有物件，idleCount為0，usingCount為0
        [Test]
        public void CheckDestory()
        {
            _objectPool = new(10);

            Test _object = null;
            byte _count = 0;
            while (_count < 5)
            {
                _count++;
                _object = _objectPool.Get();
            }

            _objectPool.Destory();

            Assert.AreEqual(0, _objectPool.idleCount);
            Assert.AreEqual(0, _objectPool.usingCount);
            Assert.AreEqual(true, _object.isDestory);
        }
    }
}


