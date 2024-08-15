using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Common.DependencyInjection
{
    public class DependencyContainer
    {
        private readonly Cache _cache;

        private readonly DependencyInjector _injector;

        public DependencyContainer()
        {
            _cache = new Cache();
            _injector = new DependencyInjector(this);
        }

        /// <summary>
        ///     Creates new and return instance, whithout injecting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Add<T>()
            where T : class
        {
            var constructors = typeof(T).GetConstructors();

            object instance;
            if (constructors.Length != 1
                || !HasDefaultConstructor(typeof(T)))
                instance = (T) FormatterServices.GetUninitializedObject(typeof(T));
            else
                instance = Activator.CreateInstance(typeof(T));

            _cache.Add(instance);
            return (T) instance;
        }

        public void Add(object target)
        {
            _cache.Add(target);
        }

        public void Add<T>(object target)
        {
            _cache.Add<T>(target);
        }

        public void AddInject(object target)
        {
            _cache.Add(target);
            _injector.Inject(target);
        }

        /// <summary>
        ///     Add and return instance, injecting its deps
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T AddInject<T>()
            where T : class
        {
            var type = typeof(T);
            var constructors = type.GetConstructors();
            object instance;

            if (constructors.Length > 1)
                throw new Exception("trying to bind multiple constructor object. can't choose.");

            if (constructors.Length == 1
                || HasDefaultConstructor(typeof(T)))
            {
                var paramInfos = constructors[0].GetParameters();
                var args = _injector.GetArguments(paramInfos);
                instance = Activator.CreateInstance(typeof(T), args);
            }
            else
            {
                instance = (T) FormatterServices.GetUninitializedObject(typeof(T));
            }

            _cache.Add(instance);
            _injector.Inject(instance);
            return (T) instance;
        }

        /// <summary>
        ///     Creates and return instance, injecting its deps
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T CreateInject<T>()
            where T : class
        {
            var type = typeof(T);
            var constructors = type.GetConstructors();
            object instance;

            if (constructors.Length > 1)
                throw new Exception("trying to bind multiple constructor object. can't choose.");

            if (constructors.Length == 1
                || HasDefaultConstructor(typeof(T)))
            {
                var paramInfos = constructors[0].GetParameters();
                var args = _injector.GetArguments(paramInfos);
                instance = Activator.CreateInstance(typeof(T), args);
            }
            else
            {
                instance = (T) FormatterServices.GetUninitializedObject(typeof(T));
            }

            return (T) instance;
        }

        public object Get(Type type)
        {
            return _cache.Get(type);
        }

        public T Get<T>()
        {
            return (T) _cache.Get(typeof(T));
        }

        public object GetList(Type type)
        {
            return _cache.GetList(type);
        }

        public List<T> GetList<T>()
        {
            if (_cache.GetList(typeof(T)) == null) return null;

            var cached = (List<object>) _cache.GetList(typeof(T));
            var list = cached.Cast<T>().ToList();
            return list;
        }

        public void Inject(object target)
        {
            _injector.Inject(target);
        }

        private bool HasDefaultConstructor(Type t)
        {
            return t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;
        }

        private class Cache
        {
            private readonly Dictionary<Type, List<object>> _objectsByInterfaces = new Dictionary<Type, List<object>>();

            private readonly Dictionary<Type, object> _objectsByTypes = new Dictionary<Type, object>();

            public void Add<T>(object target)
            {
                if (target == null)
                    throw new NullReferenceException("adding null target to di cache");

                var type = typeof(T);
                var interfaces = type.GetInterfaces();
                if (!_objectsByTypes.ContainsKey(type))
                    _objectsByTypes[type] = target;
                else
                    Console.WriteLine($"{type} is already bound to {target.GetType()} ");

                foreach (var i in interfaces)
                {
                    if (!_objectsByInterfaces.ContainsKey(i)) _objectsByInterfaces.Add(i, new List<object>());

                    if (_objectsByInterfaces[i].Contains(target))
                        Console.WriteLine($"{target.GetType()} already add to {i} list");
                    else
                        _objectsByInterfaces[i].Add(target);
                }
            }

            public void Add(object target)
            {
                var type = target.GetType();
                var interfaces = type.GetInterfaces();
                if (!_objectsByTypes.ContainsKey(type))
                    _objectsByTypes[type] = target;
                else
                    Console.WriteLine($"{type} is already bound to {target.GetType()} ");

                foreach (var i in interfaces)
                {
                    if (!_objectsByInterfaces.ContainsKey(i)) _objectsByInterfaces.Add(i, new List<object>());

                    if (_objectsByInterfaces[i].Contains(target))
                        Console.WriteLine($"{target.GetType()} already add to {i} list");
                    else
                        _objectsByInterfaces[i].Add(target);
                }
            }

            public object Get(Type type)
            {
                if (_objectsByTypes.TryGetValue(type, out var obj)) return obj;

                if (_objectsByInterfaces.TryGetValue(type, out var list))
                {
                    if (list.Count == 1) return list[0];

                    throw new Exception($"trying to resolve 1 element of type {type} from list count {list.Count}");
                }

                return null;
            }

            public object GetList(Type type)
            {
                if (_objectsByInterfaces.ContainsKey(type))
                {
                    var list = _objectsByInterfaces[type];
                    return list;
                }

                var genericListType = typeof(List<>).MakeGenericType(type);
                var genericListInstance = (IList) Activator.CreateInstance(genericListType);
                return genericListInstance;
            }
        }
    }
}