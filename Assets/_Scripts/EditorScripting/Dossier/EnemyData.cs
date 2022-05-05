using UnityEngine;

namespace CustomEditors.Dossier
{
    [CreateAssetMenu(fileName = "Enemy Info", menuName = "Scriptable Objects/Create New Dossier Entry", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public Texture icon;
        public int health;
        public int attackDamage;
        public string description;
    }
}