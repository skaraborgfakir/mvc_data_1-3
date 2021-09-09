namespace Kartotek.Modeller {
    public class Person {
	private string _namn;
	private string _bostadsort;
	private string _telefon;

	public Person( string namn,
		       string bostadsort,
		       string telefon) {
	    Namn = namn;
	    Bostadsort = bostadsort;
	    Telefon = telefon;
	}

	public string Namn {
	    get { return _namn; }
	    set { _namn = value; }
	}

	public string Bostadsort {
	    get { return _bostadsort; }
	    set { _bostadsort = value; }
	}

	public string Telefon {
	    get { return _telefon; }
	    set { _telefon = value; }
	}
    }
}
