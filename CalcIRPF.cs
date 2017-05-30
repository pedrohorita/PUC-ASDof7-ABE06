//A Funcionalidade deve receber duas informações (string tipo e double valor) pelo corpo do //request.
//Código da Funcionalidade:

using System.Net;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Set name to query string or body data
    string tipo = data.tipo;
    double valor = data.valor;

    return req.CreateResponse(HttpStatusCode.OK, getIRDevido(tipo, valor));
}


//tipo = Mensal ou Anual.
//valor = valor recebido com base no tipo. 
private static double getIRDevido(string tipo, double valor) {
    
    Const c = new Const(tipo);
    double t = 0;
    
    switch (valor) {
        case double v when (v > c.vLTer):
            t = (valor - c.vLTer) * aliConst.Qua/100;
            t += getIRDevido(tipo, c.vLTer);
            break;
        case double v when (v > c.vLSeg):
            t = (valor - c.vLSeg) * aliConst.Ter/100;
            t += getIRDevido(tipo, c.vLSeg);
            break;
        case double v when (v > c.vLPri):
            t = (valor - c.vLPri) * aliConst.Seg/100;
            t += getIRDevido(tipo, c.vLPri);
            break;
        case double v when (v > c.vBase):
            t = (valor - c.vBase) * aliConst.Pri/100;
            break;
        
    }
    return t;
    
}


private class Const {
    //Mensal, valores para cálculo
    public double vBase { get; }
    public double vLPri { get; }
    public double vLSeg { get; }
    public double vLTer { get; }
    
    public Const(string tipo) {
        if (tipo.Equals("Mensal")) {
            vBase = 1903.98; 
            vLPri = 2826.65; 
            vLSeg  = 3751.05; 
            vLTer = 4664.68;
        }
        else {
            vBase = 22847.76;
            vLPri = 33919.80;
            vLSeg  = 45012.60; 
            vLTer = 45012.60;
        }
    }
}


private static class aliConst {

    //Aliquota para cálculo
    public const double Pri = 7.5;
    public const double Seg = 15;
    public const double Ter = 22.5;
    public const double Qua = 27.5;

}
