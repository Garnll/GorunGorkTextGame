public class KeywordToStringConverter {

    private static KeywordToStringConverter instance;

    private KeywordToStringConverter() { }

    public static KeywordToStringConverter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new KeywordToStringConverter();
            }
            return instance;
        }
    }

    public string ConvertFromKeyword(DirectionKeyword key)
    {
        string direction = "";

        switch (key)
        {
            default:
                direction = "No keyword given";
            break;

            case DirectionKeyword.north:
                direction = "norte";
                break;
            case DirectionKeyword.south:
                direction = "sur";
                break;
            case DirectionKeyword.east:
                direction = "este";
                break;
            case DirectionKeyword.west:
                direction = "oeste";
                break;
            case DirectionKeyword.northEast:
                direction = "noreste";
                break;
            case DirectionKeyword.northWest:
                direction = "noroeste";
                break;
            case DirectionKeyword.southEast:
                direction = "sureste";
                break;
            case DirectionKeyword.southWest:
                direction = "suroeste";
                break;
        }

        return direction;
    }

    public DirectionKeyword ConvertFromString(string text)
    {
        DirectionKeyword keyword = DirectionKeyword.unrecognized;

        switch (text)
        {
            default:
                break;

            case "norte":
            case "n":
                keyword = DirectionKeyword.north;
                break;

            case "sur":
            case "s":
                keyword = DirectionKeyword.south;
                break;

            case "este":
            case "e":
                keyword = DirectionKeyword.east;
                break;

            case "oeste":
            case "o":
                keyword = DirectionKeyword.west;
                break;

            case "noreste":
            case "nore":
            case "ne":
                keyword = DirectionKeyword.northEast;
                break;

            case "noroeste":
            case "noro":
            case "no":
                keyword = DirectionKeyword.northWest;
                break;

            case "sureste":
            case "sure":
            case "se":
                keyword = DirectionKeyword.southEast;
                break;

            case "suroeste":
            case "suro":
            case "so":
                keyword = DirectionKeyword.southWest;
                break;
        }

        return keyword;
    }
}
