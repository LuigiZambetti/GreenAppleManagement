// JScript File
var divPopUpCorrente = null;
var timerCentratura = null;
function SetAlternateStyleTableRows(idHTMLTable)
{
    //questa funzione cicla la griglia passata e setta l'alternate sullo stile delle righe
    var objTable = document.getElementById(idHTMLTable);
    if (objTable != null)
    {
        var Alternate = false;
        for (var i=0;i<=objTable.rows.length-1;i++)
        {
            if (Alternate == false)
            {        
                objTable.rows[i].className = "webgridRowStyleDefault";
                Alternate = true;
            }
            else
            {
                objTable.rows[i].className = "webgridRowAlternateStyleDefault";
                Alternate = false;
            }
        }
    }
}

function SetPositionBottomDivPopUp(objdivMessageBox,X,Y)
{   
    //questa funzione inizializza la posizione del div modale centrandolo nello schermo 
    //settando la proprietà "nascondecombo" a true
    if (objdivMessageBox != null)
    {
        var L = (document.body.offsetWidth/2) - (X/2);
        var T = GetTopDiv(Y,Y);                
        objdivMessageBox.style.left= L;
        objdivMessageBox.style.top = document.body.offsetTop + Y + 50;
        objdivMessageBox.style.height= Y;
        objdivMessageBox.style.width= X;                  
        objdivMessageBox.nascondecombo = true;           
        objdivMessageBox.style.zIndex = 10;  
    }
}

function SetPositionCenterDivPopUp(objdivMessageBox,X,Y)
{   
    //questa funzione inizializza la posizione del div modale centrandolo nello schermo 
    //settando la proprietà "nascondecombo" a true
    if (objdivMessageBox != null)
    {
    
        var L = (document.body.offsetWidth/2) - (X/2);
        var T = GetTopDiv(Y,Y);                
        objdivMessageBox.style.left= L;
        objdivMessageBox.style.top= T;
        objdivMessageBox.style.height= Y;
        objdivMessageBox.style.width= X;                  
        objdivMessageBox.nascondecombo = true;           
        objdivMessageBox.style.zIndex = 10;  
    }
}
function GetTopDiv(HEIGHT_DIV_IE, HEIGHT_DIV_FIREFOX)
{
    //questa funzione accetta in ingresso l'altezza del div (1° parametro per IE e 2° parametro per Firefox)
    //restituisce il top centrato nello schermo compreso di posizione scrollbar
    var T=0;
    try
	{
        if (document.all) //IE
        {
            T = (document.body.offsetHeight/2) - (HEIGHT_DIV_IE/2) + document.body.scrollTop;                            
        }
        else // NOT IE
        {
            T = (document.body.innerHeight/2) - (HEIGHT_DIV_FIREFOX/2) + (window.pageYOffset/2);            
        }
    }
    catch(e)
    {            
    }  
    return T;  
}
function SetHandleDrag(theHandle,theRoot)
{ 
    //questa funzione inizializza il drag del div passato come argomento
    if (theHandle != null && theRoot != null)
    {
         Drag.init(theHandle, theRoot);
         theHandle.style.cursor='move';
	}
}
function GetStatusModalDivAbsPos() 
{        
    //questa funzione se rileva un DIV nella pagina con la proprietà "nascondecombo" settata a true esegue i seguenti step:
    //cicla tutti gli oggetti di tipo SELECT
    //l'elemento SELECT viene nascosto solo se tra i suoi parent NON esiste il div con la proprietà nascondecombo settata a true
    var TrovatoElementoModale = false;        
    var elementsDIV = document.documentElement.getElementsByTagName('div');
    for (var i=0; i<elementsDIV.length; i++) {
        if (elementsDIV[i].nascondecombo != null && elementsDIV[i].nascondecombo == true)
        {
            TrovatoElementoModale = true;
            break;
        }
    }
    if (TrovatoElementoModale)
    {
        var previousParent = 'startup';
        //inizio del ciclo su tutti gli elementi della pagina
        var elements = document.documentElement.getElementsByTagName('SELECT');                        
        
        
        for (var i=0; i<elements.length; i++) 
        {
           
            var pDaNascondere = true;
            
            //verifico se tra i parent esiste un elemento con la proprietà "nascondecombo" settata a true
            var pParentElement = elements[i].parentNode;
            while (pParentElement != null && pDaNascondere == true)
            {            
                if (pParentElement.nascondecombo != null && pParentElement.nascondecombo == true)
                {                                                                                
                    pDaNascondere = false;
                    pParentElement = null;                                
                }
                else
                {
                    pParentElement = pParentElement.parentNode;                    
                }                
            }
            
            if (pDaNascondere)//se tra i parent dell'elemento in ciclo è stato trovato un elemento con la proprietà "nascondecombo" settata a true
            {
                //se l'elemento in ciclo è di tipo select
                if (elements[i].type=='select-one')
                {
                    if (typeof document.body.style.maxHeight != "undefined") 
                    {
                      // IE 7, mozilla, safari, opera 9
                      //elements[i].disabled = true; //disabilito l'elemento select
                    } else {
                      // IE6, older browsers
                      elements[i].style.display = "none"; //nascondo l'elemento select                                                                           
                    }             
                }
                var tableelements = elements[i].ownerDocument.documentElement.getElementsByTagName("A");
                Modalizza(tableelements);
                tableelements = elements[i].ownerDocument.documentElement.getElementsByTagName("INPUT");
                Modalizza(tableelements);
                tableelements = elements[i].ownerDocument.documentElement.getElementsByTagName("IMG");
                Modalizza(tableelements);
                tableelements = elements[i].ownerDocument.documentElement.getElementsByTagName("TEXTAREA");
                Modalizza(tableelements);
                tableelements = elements[i].ownerDocument.documentElement.getElementsByTagName("TABLE");
                Opacizza(tableelements);
                tableelements = elements[i].ownerDocument.documentElement.getElementsByTagName("DIV");
                Opacizza(tableelements);
                //lancio un timer che dopo 500 millisecondi esegue una funzione che centra il div nello schermo
                if (timerCentratura == null)
                {                
                    timerCentratura = window.setTimeout("centra_divPopUpCorrente()",500);                          
                }
            }
            else
            {
                if (elements[i].type=='select-one')
                {
                    elements[i].style.display = "";
                    elements[i].disabled = false;
                }                                      
            }
            
        }
    }
} 
function centra_divPopUpCorrente()
{    
    //questa funzione centra il div modale corrente nello schermo
    if (divPopUpCorrente != null)
    {
		var pHeightIE = 0;
		var pHeightFY = 0;
		
		
		if (document.all) //IE
        {
			pHeightIE = divPopUpCorrente.offsetHeight;
		}
        else // NOT IE
        {
			pHeightFY = 100;		
		}			
		try
		{
        var T = GetTopDiv(pHeightIE,pHeightFY);                        
        divPopUpCorrente.style.top = T;
		}catch(e){}
    }
    window.clearTimeout(timerCentratura);
    timerCentratura = null;
}
function Opacizza(tableelements)
{
    return;
    for (var t=0; t<tableelements.length; t++) 
    {                                                     
        if (tableelements[t].nascondecombo == null || tableelements[t].nascondecombo == false)
        {
            //verifico se tra i parent dell'elemento in ciclo esiste un elemento con la proprietà "nascondecombo" settata a true                
            var pDaNascondere2 = true;
            var pParentElement2 = tableelements[t].parentNode;
            while (pParentElement2 != null)
            {            
                if (pParentElement2.nascondecombo != null && pParentElement2.nascondecombo == true)
                {
                    pDaNascondere2 = false;
                    divPopUpCorrente = pParentElement2;                                        
                    pParentElement2 = null;                                                        
                }
                else
                {
                    pParentElement2 = pParentElement2.parentNode;                    
                }                
            }
            if (pDaNascondere2)//se tra i parent dell'elemento in ciclo è stato trovato un elemento con la proprietà "nascondecombo" settata a true
            {                
                tableelements[t].style.opacity= 0.4; //opacizzo l'elemento in firefox
                tableelements[t].style.filter += "alpha(opacity=40);"//opacizzo l'elemento in IE
            }
        }
    }        
}
function Modalizza(tableelements)
{    
    return;
    for (var t=0; t<tableelements.length; t++) 
    {                                                     
        if (tableelements[t].nascondecombo == null || tableelements[t].nascondecombo == false)
        {        
            //verifico se tra i parent dell'elemento in ciclo esiste un elemento con la proprietà "nascondecombo" settata a true                
            var pDaNascondere2 = true;
            var pParentElement2 = tableelements[t].parentNode;
            while (pParentElement2 != null)
            {            
                if (pParentElement2.nascondecombo != null && pParentElement2.nascondecombo == true)
                {
                    pDaNascondere2 = false;
                    divPopUpCorrente = pParentElement2;                                            
                    pParentElement2 = null;                                                        
                }
                else
                {
                    pParentElement2 = pParentElement2.parentNode;                    
                }                
            }
            if (pDaNascondere2)//se tra i parent dell'elemento in ciclo è stato trovato un elemento con la proprietà "nascondecombo" settata a true
            {                
                tableelements[t].onclick = onclickNullo; //annullo il click sull'elemento
                tableelements[t].onkeydown = onkeydownNullo;//annullo il keydown sull'elemento
            }
        }
    }
}
function UnOpacizza(tableelements)
{
    return;
    for (var t=0; t<tableelements.length; t++) 
    {                                                     
        if (tableelements[t].nascondecombo == null || tableelements[t].nascondecombo == false)
        {
            //verifico se tra i parent dell'elemento in ciclo esiste un elemento con la proprietà "nascondecombo" settata a true                
            var pDaNascondere2 = true;
            var pParentElement2 = tableelements[t].parentNode;
            while (pParentElement2 != null)
            {            
                if (pParentElement2.nascondecombo != null && pParentElement2.nascondecombo == true)
                {
                    pDaNascondere2 = false;
                    divPopUpCorrente = pParentElement2;                                        
                    pParentElement2 = null;                                                        
                }
                else
                {
                    pParentElement2 = pParentElement2.parentNode;                    
                }                
            }
            if (pDaNascondere2)//se tra i parent dell'elemento in ciclo è stato trovato un elemento con la proprietà "nascondecombo" settata a true
            {                
                tableelements[t].style.opacity= "";//ripristino l'opacità originaria sull'elemento in ciclo
                tableelements[t].style.filter = ""//ripristino l'opacità originaria sull'elemento in ciclo
            }
        }
    }
}
function UnModalizza(tableelements)
{    
    return;
    for (var t=0; t<tableelements.length; t++) 
    {                                                     
        if (tableelements[t].nascondecombo == null || tableelements[t].nascondecombo == false)
        {        
            //verifico se tra i parent dell'elemento in ciclo esiste un elemento con la proprietà "nascondecombo" settata a true                
            var pDaNascondere2 = true;
            var pParentElement2 = tableelements[t].parentNode;
            while (pParentElement2 != null)
            {            
                if (pParentElement2.nascondecombo != null && pParentElement2.nascondecombo == true)
                {
                    pDaNascondere2 = false;
                    divPopUpCorrente = pParentElement2;                                            
                    pParentElement2 = null;                                                        
                }
                else
                {
                    pParentElement2 = pParentElement2.parentNode;                    
                }                
            }
            if (pDaNascondere2)//se tra i parent dell'elemento in ciclo è stato trovato un elemento con la proprietà "nascondecombo" settata a true
            {                
                tableelements[t].onclick = "";  //ripristino l'opacità originaria sull'elemento in ciclo                      
                tableelements[t].onkeydown = "";//ripristino l'opacità originaria sull'elemento in ciclo
            }
        }
    }
}
var ContaBrilli = 0;
var OggettoBrilli;
var OriginBorderColor;
var isActiveBrilli;
function onclickNullo()
{
    if (divPopUpCorrente != null)
    {
        if (isActiveBrilli == null)
        {
           TentaBrillo();
        }
    }
    return false;
}
function onkeydownNullo()
{
    if (divPopUpCorrente != null)
    {
        TentaBrillo();
    }
    try
    {
        window.event.keyCode = null;
    }
    catch(e)
    {}
    return false;
}
function TentaBrillo()
{
    if (isActiveBrilli == null)
    {
        ContaBrilli = 0;
        OggettoBrilli = window.setTimeout("brilla()",150);
        OriginBorderColor = divPopUpCorrente.style.borderColor;       
		var T = GetTopDiv(divPopUpCorrente.offsetHeight,divPopUpCorrente.style.height);                                
        divPopUpCorrente.style.top = T;   
    }
}
function brilla()
{    
    if (typeof document.body.style.maxHeight != "undefined") 
    {
        // IE 7, mozilla, safari, opera 9
    } 
    else 
    {
        // IE6, older browsers
        var elements = document.getElementsByTagName('select');
        for (var i=0; i<elements.length; i++) 
        {            
            elements[i].style.display = "none";
        }                                                                          
    }    
    isActiveBrilli = true;
    if (divPopUpCorrente.style.borderColor!=OriginBorderColor)
    {        
        divPopUpCorrente.style.borderColor=OriginBorderColor;
    }
    else
    {
        divPopUpCorrente.style.borderColor="#e9e9e9";
    }
    if (ContaBrilli>5)
    {
        ContaBrilli = 0;
        divPopUpCorrente.style.borderColor=OriginBorderColor;
        window.clearTimeout(OggettoBrilli);
        isActiveBrilli = null;
    }
    else
    {
        ContaBrilli+=1;
        window.setTimeout("brilla()",150);
    }
}
function InitCustomSettings()
{       
    //window.setTimeout("renewSession()",1000);      
}      
function renewSession()
{
   //document.location.href=document.location.href; 
}
