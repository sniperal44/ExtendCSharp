namespace ExtendCSharp.Interfaces
{
    //l'interfaccia ICastable permette di effettuare il Cast di un Object nel tipo di oggetto corrente in base alla funzione Cast
    //la funzione SelfCast permette di eseguire la medesima operazione ma portando gli effetti del cast sull'oggetto chiamante
    interface ICastable
    {
        object Cast(object o);
        void SelfCast(object o);
    }
    //------------------------------------------------------
    //------------------------------------------------------
    //ATTENZIONE! IMPLEMENTARE SEMPRE UN COSTRUTTORE VUOTO!
    //------------------------------------------------------
    //------------------------------------------------------
}
