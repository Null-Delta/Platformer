public class Grass : ConnectedObject
{
    public override string objectName => "Grass";
    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        gameObject.transform.position = position;
        setupStyle((int)position.x, (int)position.y);
        order = ObjectOrder.onFloor;
    }

    public override void execute(Command command)
    {

    }

    public Grass(int x, int y): base(x,y) { }

    public Grass():base(0,0) {}
}
