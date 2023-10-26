<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PushData.aspx.cs" Inherits="PRAGATIVTSMVC.PushData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
  
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script  type="text/javascript">
        $(document).ready(function() {
            setInterval("location.reload(true)", 300000);
        });   
    </script>
</head> 
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="lbl_message" runat="server"></asp:Label>
    </div>
    </form>
    <script>
        //window.setInterval(function () {
        //    // this will execute every 1 second

        //    methodCallOrAction();

        //}, 60000*5);



        //function methodCallOrAction() {
        //    // u can call an url or do something here
        //    // location.href = 'http://tranopro.com/pushdata.aspx';
        //    location.reload();
        //} 
    </script>
</body>
</html>
