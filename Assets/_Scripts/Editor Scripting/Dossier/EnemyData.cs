using UnityEngine;

namespace CustomEditors.Dossier
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Scriptable Objects/Dossier/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public Texture icon;
        public int health;
        public int attackDamage;
        public string description;
    }
}