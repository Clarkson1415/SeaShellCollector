namespace Assets.Scripts.Items
{

    public class CapacityEffect : ItemEffect
    {
        public override void Apply(Player player)
        {
            player.MaxCapacity += this.Value;
        }
    }
}
