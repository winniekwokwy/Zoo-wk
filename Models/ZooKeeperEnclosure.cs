
public class ZooKeeperEnclosure
{
    public int Id { get; set; }
    public required int ZooKeeperId { get; set; }
    public ZooKeeper? zookeeper { get; set; }
    public required int EnclosureId { get; set; }
    public Enclosure? enclosure { get; set; }
}