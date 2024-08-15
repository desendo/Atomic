using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExample.Scripts.Data
{
    [Serializable]
    public class VisualData : IData
    {

        public List<ObjectEntry<Sprite>> Sprites;
        public List<ObjectEntry<AudioClip>> SoundsConfirm;
        public List<ObjectEntry<AudioClip>> SoundsSelect;
        public List<ObjectEntry<AudioClip>> SoundsMove;
        public List<ObjectEntry<AudioClip>> SoundsAttack;

        private Dictionary<string, Sprite> _cacheSprites = new Dictionary<string, Sprite>();




        public Sprite GetSprite(string id)
        {
            if (_cacheSprites.TryGetValue(id, out var obj))
                return obj;

            var target = Sprites.FirstOrDefault(x => x.Id == id)?.Obj;
            if (target != null)
                _cacheSprites.Add(id, target);
            return target;
        }
    }

    [Serializable]
    public class ObjectEntry<T>
    {
        public string Id;
        public T Obj;
    }

    public static class RandomUtils
    {
    }
}