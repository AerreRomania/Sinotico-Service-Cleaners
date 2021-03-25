<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Work.aspx.cs" Inherits="Work"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Work</title>
        <link rel="stylesheet" href="Styles/tablestatcss.css" />

    <link rel="stylesheet" href="Styles/work_style.css" />
    <link rel="stylesheet" href="Styles/bootstrap.min.css" />
    <link href="https://fonts.googleapis.com/css?family=Titillium+Web" rel="stylesheet" />
    <link rel="stylesheet" href="Styles/statcss.css" />
      <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>

    <style>
        .brmachine{
           position: absolute;
            font-size: 9px;
            padding-left: 35px;
        }
          .results tr[visible='false'], .no-result {
                display: none;
            }

            .results tr[visible='true'] {
                display: table-row;
            }

            .stopwatch .controls {
                    font-size: 12px;
                }

            .rgMasterTable th{
                    text-align: center;
                    text-transform: uppercase;
                    padding-left:5px;
                    padding-right:5px;
            }

             .rgMasterTable td{
                    text-align: center;
                    padding-left:5px;
                    padding-right:5px;
                        border: 1px solid #c7c7c7;
            }

          .hdr {
              border-bottom: 1px solid #d0d3d7;
              border-top: 20px solid #f7f6f3; 
          }

          .tdb {
              border-bottom: 1px solid #cccfd3;
          }

          .name {
              min-width:150px;
          }
           

           #button_mail{
               position:fixed;
               bottom:0;
               right:10%; 
           }

            /*#button_image{
               position:fixed;
               bottom:0;
               right:15%;
           }*/

            .red{
                 border: 1px solid red;
                box-shadow: 1px 1px red;

            }
            .yellow{
                    border: 1px solid #dead71;
                    box-shadow: 1px 1px #ffc800;
            }
            .green{
                 border: 1px solid green;
                  box-shadow: 1px 1px green;
            }

            .nonExpense{
                 display: inline-block;
                    padding: 10px;
                    /*border: 1px solid #b7bbc1;*/
                    border-radius: 5px;
                    margin: 5px;
                    text-align: center;
                    width: 50px;
                    /*box-shadow: 1px 1px #c3c7cb;*/
            }
            span{
                font-size:14px;
                white-space:nowrap;
                text-align:center;
            }

            .center {
              margin: auto;
              width: 50%;
              /*border: 3px solid green;*/
              padding: 10px;
            }

            h4{
                       text-align: center;
               }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data" method="post">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Scripts/jquery-3.3.1.min.js" />
             <asp:ScriptReference Path="~/Scripts/jquery-ui-min.js" />
            </Scripts>
        </asp:ScriptManager>

        <asp:UpdatePanel runat="server" ID="upd1"  > 
            <Triggers>
              <asp:PostBackTrigger ControlID="button_mail" />
               <asp:PostBackTrigger ControlID="btn_sendImage" />
             </Triggers>
            <ContentTemplate> 

                <meta content='width=device-width, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no' name='viewport'>
                <header id='header'>
                    <div class='wrapper'>
                        <h1>
                            <a href='!#' class="not-active">Olimpias Knitting
                            </a>
                        </h1>
                        <div id='notifications'>
                            <div id='indicator'>
                                <span>notifications</span>
                                <a class='count important' href='!#' runat="server" id="btnLogoutHelper" onclick="$('#btnLogout').click();return false;">Log Out</a>
                                <asp:Button runat="server" ID="btnLogout" OnClick="LogOut" style="display:none;"/>
                                <br/>
                                <asp:UpdateProgress id="updateProgress" runat="server">
                                    <ProgressTemplate>
                                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                                            <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" class="lds-gears" width="159px" height="159px" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid" style="background: none;"><g transform="translate(50 50)"> <g transform="translate(-19 -19) scale(0.6)"> <g transform="rotate(238.763)">
<animateTransform attributeName="transform" type="rotate" values="0;360" keyTimes="0;1" dur="2.3s" begin="0s" repeatCount="indefinite"/><path d="M37.3496987939662 -7 L47.3496987939662 -7 L47.3496987939662 7 L37.3496987939662 7 A38 38 0 0 1 31.359972760794346 21.46047782418268 L31.359972760794346 21.46047782418268 L38.431040572659825 28.531545636048154 L28.531545636048154 38.431040572659825 L21.46047782418268 31.359972760794346 A38 38 0 0 1 7.0000000000000036 37.3496987939662 L7.0000000000000036 37.3496987939662 L7.000000000000004 47.3496987939662 L-6.999999999999999 47.3496987939662 L-7 37.3496987939662 A38 38 0 0 1 -21.46047782418268 31.35997276079435 L-21.46047782418268 31.35997276079435 L-28.531545636048154 38.431040572659825 L-38.43104057265982 28.531545636048158 L-31.359972760794346 21.460477824182682 A38 38 0 0 1 -37.3496987939662 7.000000000000007 L-37.3496987939662 7.000000000000007 L-47.3496987939662 7.000000000000008 L-47.3496987939662 -6.9999999999999964 L-37.3496987939662 -6.999999999999997 A38 38 0 0 1 -31.35997276079435 -21.460477824182675 L-31.35997276079435 -21.460477824182675 L-38.431040572659825 -28.531545636048147 L-28.53154563604818 -38.4310405726598 L-21.4604778241827 -31.35997276079433 A38 38 0 0 1 -6.999999999999992 -37.3496987939662 L-6.999999999999992 -37.3496987939662 L-6.999999999999994 -47.3496987939662 L6.999999999999977 -47.3496987939662 L6.999999999999979 -37.3496987939662 A38 38 0 0 1 21.460477824182686 -31.359972760794342 L21.460477824182686 -31.359972760794342 L28.531545636048158 -38.43104057265982 L38.4310405726598 -28.53154563604818 L31.35997276079433 -21.4604778241827 A38 38 0 0 1 37.3496987939662 -6.999999999999995 M0 -23A23 23 0 1 0 0 23 A23 23 0 1 0 0 -23" fill="#fbca00"/></g></g> <g transform="translate(19 19) scale(0.6)"> <g transform="rotate(98.7369)">
<animateTransform attributeName="transform" type="rotate" values="360;0" keyTimes="0;1" dur="2.3s" begin="-0.14375s" repeatCount="indefinite"/><path d="M37.3496987939662 -7 L47.3496987939662 -7 L47.3496987939662 7 L37.3496987939662 7 A38 38 0 0 1 31.359972760794346 21.46047782418268 L31.359972760794346 21.46047782418268 L38.431040572659825 28.531545636048154 L28.531545636048154 38.431040572659825 L21.46047782418268 31.359972760794346 A38 38 0 0 1 7.0000000000000036 37.3496987939662 L7.0000000000000036 37.3496987939662 L7.000000000000004 47.3496987939662 L-6.999999999999999 47.3496987939662 L-7 37.3496987939662 A38 38 0 0 1 -21.46047782418268 31.35997276079435 L-21.46047782418268 31.35997276079435 L-28.531545636048154 38.431040572659825 L-38.43104057265982 28.531545636048158 L-31.359972760794346 21.460477824182682 A38 38 0 0 1 -37.3496987939662 7.000000000000007 L-37.3496987939662 7.000000000000007 L-47.3496987939662 7.000000000000008 L-47.3496987939662 -6.9999999999999964 L-37.3496987939662 -6.999999999999997 A38 38 0 0 1 -31.35997276079435 -21.460477824182675 L-31.35997276079435 -21.460477824182675 L-38.431040572659825 -28.531545636048147 L-28.53154563604818 -38.4310405726598 L-21.4604778241827 -31.35997276079433 A38 38 0 0 1 -6.999999999999992 -37.3496987939662 L-6.999999999999992 -37.3496987939662 L-6.999999999999994 -47.3496987939662 L6.999999999999977 -47.3496987939662 L6.999999999999979 -37.3496987939662 A38 38 0 0 1 21.460477824182686 -31.359972760794342 L21.460477824182686 -31.359972760794342 L28.531545636048158 -38.43104057265982 L38.4310405726598 -28.53154563604818 L31.35997276079433 -21.4604778241827 A38 38 0 0 1 37.3496987939662 -6.999999999999995 M0 -23A23 23 0 1 0 0 23 A23 23 0 1 0 0 -23" fill="#d77452"/></g></g></g></svg>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>

                        </div>
                        <asp:Label runat="server" ID="lblName" CssClass="username"></asp:Label>
                        <asp:Label runat="server" ID="lblOpId" style="display:none;"></asp:Label>
                        <asp:Label runat="server" ID="lblJobType" style="display:none;"></asp:Label>
                        <asp:Label runat="server" ID="lblhelper" style="display:none;">helper</asp:Label>
                        <asp:Label runat="server" ID="lblMod" style="display:none;" >test</asp:Label> 
                        <asp:Label runat="server" ID="lblCleanersType" style="display:none;"></asp:Label>
                        <asp:Label runat="server" ID="lblstartdate" style="display:none;"></asp:Label>
                        <asp:Label runat="server" ID="lblturno" style="display:none">True</asp:Label>
                       
                         
                        <nav>
                            <ul id='menu'>
                                <li> 
                                    <asp:DropDownList runat="server" style="display:none;" ID="org">
                                        <asp:ListItem Value="Pulizia Ordinaria" Text="Pulizia Ordinaria"></asp:ListItem>
                                        <asp:ListItem Value="Pulizia Fronture" Text="Pulizia Fronture"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>

                                <li>
                                    <a  href='#' id="idhome" class="active" runat="server">Home</a>
                                </li>

                                <%-- CQ --%>
                                <li><a href="#" style="display:none;" runat="server" id="id_layout_cq_eff" >Lay.Eff</a></li>
                                <li><a href="#" style="display:none;" runat="server" id="id_layout_cq_pulizia" >Lay.Pulizia</a></li>
                                <li><a href="#" style="display:none;" runat="server" id="id_layout_cq_cq">Lay.CQ</a></li>
                                <li><a href="#" style="display:none;" runat="server" id="id_layout_cq_alarms">CQ Alarms</a></li>
                                <li><a href="#" style="display:none;" runat="server" id="id_myJob">My Job</a></li>

                                <%-- PULIZIA --%>
                                <li><a href="#" style="display:none;" runat="server" id="id_layout_pul_pul">Lay.Pulizia</a></li>
                                <li><a href="#" style="display:none;" runat="server" id="id_layout_pul_alarms">Pul.Alarms</a></li> 
                            </ul>
                        </nav>
                        <a class='menu-link' href='#menu'>Menu</a>
                    </div>
                </header>

            <asp:Button runat="server" id="button_home" style="display: none" OnClick="button_home_OnClick"/>
            <asp:Button runat="server" id="button_lay_eff" style="display: none" OnClick="button_lay_eff_OnClick"/>
            <asp:Button runat="server" id="button_lay_pul" style="display: none" OnClick="button_lay_pul_OnClick"/>
            <asp:Button runat="server" ID="button_lay_cq" style="display: none" OnClick="button_lay_cq_OnClick"/>
            <asp:Button runat="server" id="button_cq_alarms" style="display: none" OnClick="button_cq_alarms_OnClick"/>
            <asp:Button runat="server" id="button_myJob" style="display: none" OnClick="button_myJob_Click"/>

            <asp:Button runat="server" ID="button_pul_alarms" style="display: none" OnClick="button_pul_alarms_OnClick"/>

            <asp:ImageButton runat="server" ID="button_mail" ImageUrl="~/Images/email.png" OnClick="button_mail_Click" />


            <asp:Panel runat="server" ID="panel_home">
                <div id="pnlhome" runat="server">
                    
                    <asp:Table runat="server" CssClass="container-fluid">
                        <asp:TableRow Style="height: 70px;">
                            <asp:TableCell>
                                    <asp:Button runat="server" Width="210" Height="50" style="margin-top: 15px;" CssClass="btnconfirm btn btn-3" ID="btnConfirm"   Text="START" OnClick="btnConfig_Click"/>
                                    <asp:Label runat="server" id="lbl_error" style="font-weight: 600;color: red;font-size: 20px;"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="txt_error"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell CssClass="cell-right">
                                    <asp:Button runat="server" ID="btnStop" Width="210" Height="50" CssClass="btnstop btn btn-3 reset" Text="STOP" OnClick="btnStop_Click" />
                                  
                            </asp:TableCell>
                            <asp:TableCell CssClass="cell-right-right">
                                 <asp:Button runat="server"  ID="btnAlarm" CssClass="btnallarma btn btn-3-alarma" OnClick="btnAlarm_Click"  Text="Allarme" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow CssClass="main_body">
                            <asp:TableCell></asp:TableCell>

                        </asp:TableRow>
                    </asp:Table>
                    <br />
                    <asp:Panel runat="server" ID="panel_home_labels">
                    <div class="container" id="home_labels">
                        <div class="row">
                            <div class="col-xs-2 spans">Macchina:</div>
                            <div class="col-xs-10"><asp:TextBox runat="server" CssClass="col-xs-12" ID="txtMachina" TextMode="Number" AutoPostBack="True" ></asp:TextBox></div>
                        </div>
                        <br />   
                            <div class="row">
                            <div class="col-xs-2 spans">Progressivo:</div>
                            <div class="col-xs-10"><asp:TextBox CssClass="improvised col-xs-12" Enabled="false"  runat="server" ID="lblProgressivo"></asp:TextBox></div>
                        </div>

                            <br />   
                            <div class="row" id="idnota">
                                <asp:label style="display:none;" runat="server" ID ="tester"></asp:label>
                            <div class="col-xs-2 spans">Nota Alarm:</div>
                            <div class="col-xs-10"><asp:TextBox CssClass="improvised col-xs-12" Height="100"  runat="server"  ID="lblnote"></asp:TextBox></div>
                        </div>

                    </div>
                    </asp:Panel>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="panel_cqlayout">
            <div id="pnllayout" runat="server" >

                     <div>
           <asp:Table ID="selectMenu" runat="server" Style="background-color:#e1e0dd;width:100%">
                <asp:TableRow ID="Col1">
                    <asp:TableCell CssClass="tc1"> Data Inizio
                    </asp:TableCell>
                    <asp:TableCell CssClass="tc1"> 

       <asp:TextBox ID="startDate" runat="server" type="date" date-format="dd/MM/yyyy"></asp:TextBox>

                    </asp:TableCell>
                    <asp:TableCell CssClass="tc1"> Data Fine
                    </asp:TableCell>
                    <asp:TableCell CssClass="tc1"> 

       <asp:TextBox ID="endDate" runat="server" type="date" ></asp:TextBox>

                    </asp:TableCell>
                     <asp:TableCell CssClass="tc1"> Turno
                    </asp:TableCell>
                    <asp:TableCell CssClass="tc1"> 
                        <asp:DropDownList ID="turno" runat="server" >
                            <%--<asp:ListItem Text="" Value=""> </asp:ListItem>--%>
                            <asp:ListItem Text="Turno 1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Turno 2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Turno 3" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" ID="bobitester" style="display:none;"></asp:Label>
 
                     <%--   <p>StartDate: <input type="text" id="start_datepicker" runat="server"/></p>
                        <p>EndDate: <input type="text" id="end_datepicker" runat="server"/></p>--%>

                    </asp:TableCell>
                    <asp:TableCell  CssClass="tc1">
                    </asp:TableCell>
                    <asp:TableCell CssClass="tc1">
                       <%-- <asp:button ID="total" runat="server" Text="78"> 

                        </asp:button>--%>
                        <span>Total:</span>
                        <asp:Label runat="server" ID="total"></asp:Label>


                    </asp:TableCell>
                        <asp:TableCell CssClass="tc1">
                             <asp:Button ID="aggiorna_eff" runat="server" Text="Aggiorna" OnClick="aggiorna_eff_Click" />
                            <asp:Button ID="aggiorna_pul" runat="server" Text="Aggiorna" OnClick="aggiorna_pul_Click" />
                            <asp:Button ID="aggiorna_cq" runat="server" Text="Aggiorna" OnClick="aggiorna_cq_Click" />

                    </asp:TableCell>
                </asp:TableRow>
            
            </asp:Table>

                        <asp:Table ID="mainBody" Style="width:100%" runat="server" >
               <asp:TableRow CssClass="titleRow" ID="titleRow1">
                    <asp:TableCell CssClass="titleLabelCell" columnspan="10" style="text-align:center">
                        <asp:Label ID="s1" CssClass="titleLabel" runat="server">

                            VA 14 / MM 13 (27) s1

                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
         
                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p1" CssClass="machineLabel"  runat="server">1</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p14" CssClass="machineLabel"  runat="server">14</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p15" CssClass="machineLabel"  runat="server">15</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p28" CssClass="machineLabel"  runat="server">28</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p29" CssClass="machineLabel"  runat="server">29</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p42" CssClass="machineLabel"  runat="server">42</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p43" CssClass="machineLabel"  runat="server">43</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p56" CssClass="machineLabel"  runat="server">56</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p57" CssClass="machineLabel"  runat="server">57</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p70" CssClass="machineLabel"  runat="server">
                            70
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p2" CssClass="machineLabel"  runat="server">2</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p13" CssClass="machineLabel"  runat="server">13</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p16" CssClass="machineLabel"  runat="server">16</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p27" CssClass="machineLabel"  runat="server">27</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p30" CssClass="machineLabel"  runat="server">30</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p41" CssClass="machineLabel"  runat="server">41</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p44" CssClass="machineLabel"  runat="server">44</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p55" CssClass="machineLabel"  runat="server">55</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p58" CssClass="machineLabel"  runat="server">58</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p69" CssClass="machineLabel"  runat="server">
                            69
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p3" CssClass="machineLabel"  runat="server">3</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p12" CssClass="machineLabel"  runat="server">12</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p17" CssClass="machineLabel"  runat="server">17</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p26" CssClass="machineLabel"  runat="server">26</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p31" CssClass="machineLabel"  runat="server">31</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p40" CssClass="machineLabel"  runat="server">40</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p45" CssClass="machineLabel"  runat="server">45</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p54" CssClass="machineLabel"  runat="server">54</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p59" CssClass="machineLabel"  runat="server">59</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p68" CssClass="machineLabel"  runat="server">
                            68
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p4" CssClass="machineLabel"  runat="server">4</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p11" CssClass="machineLabel"  runat="server">11</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p18" CssClass="machineLabel"  runat="server">18</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p25" CssClass="machineLabel"  runat="server">25</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p32" CssClass="machineLabel"  runat="server">32</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p39" CssClass="machineLabel"  runat="server">39</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p46" CssClass="machineLabel"  runat="server">46</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p53" CssClass="machineLabel"  runat="server">53</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p60" CssClass="machineLabel"  runat="server">60</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p67" CssClass="machineLabel"  runat="server">
                            67
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p5" CssClass="machineLabel"  runat="server">5</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p10" CssClass="machineLabel"  runat="server">10</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p19" CssClass="machineLabel"  runat="server">19</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p24" CssClass="machineLabel"  runat="server">24</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p33" CssClass="machineLabel"  runat="server">33</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p38" CssClass="machineLabel"  runat="server">38</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p47" CssClass="machineLabel"  runat="server">47</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p52" CssClass="machineLabel"  runat="server">52</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p61" CssClass="machineLabel"  runat="server">61</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p66" CssClass="machineLabel"  runat="server">
                            66
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p6" CssClass="machineLabel"  runat="server">6</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p9" CssClass="machineLabel"  runat="server">9</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p20" CssClass="machineLabel"  runat="server">20</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p23" CssClass="machineLabel"  runat="server">23</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p34" CssClass="machineLabel"  runat="server">34</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p37" CssClass="machineLabel"  runat="server">37</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p48" CssClass="machineLabel"  runat="server">48</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p51" CssClass="machineLabel"  runat="server">51</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p62" CssClass="machineLabel"  runat="server">62</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p65" CssClass="machineLabel"  runat="server">
                            65
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p7" CssClass="machineLabel"  runat="server">7</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p8" CssClass="machineLabel"  runat="server">8</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p21" CssClass="machineLabel"  runat="server">21</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p22" CssClass="machineLabel"  runat="server">22</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p35" CssClass="machineLabel"  runat="server">35</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p36" CssClass="machineLabel"  runat="server">36</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p49" CssClass="machineLabel"  runat="server">49</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p50" CssClass="machineLabel"  runat="server">50</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p63" CssClass="machineLabel"  runat="server">63</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p64" CssClass="machineLabel"  runat="server">
                            64
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l1" CssClass="machineLabel2"  runat="server">1</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l2" CssClass="machineLabel2"  runat="server">2</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l3" CssClass="machineLabel2"  runat="server">3</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l4" CssClass="machineLabel2"  runat="server">4</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l5" CssClass="machineLabel2"  runat="server">5</asp:Label>
                    </asp:TableCell>
                             
          
                </asp:TableRow>
            </asp:Table>

                        <asp:Table ID="Table1" Style="width:100%" runat="server" >
               <asp:TableRow CssClass="titleRow" ID="titleRow2">
                    <asp:TableCell CssClass="titleLabelCell" columnspan="10" style="text-align:center">
                        <asp:Label ID="s2" CssClass="titleLabel" runat="server">

                            VA 14 / MM 13 (27) s2

                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
         
                 <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p71" CssClass="machineLabel"  runat="server">71</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p84" CssClass="machineLabel"  runat="server">14</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p85" CssClass="machineLabel"  runat="server">15</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p98" CssClass="machineLabel"  runat="server">28</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p99" CssClass="machineLabel"  runat="server">29</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p112" CssClass="machineLabel"  runat="server">42</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p113" CssClass="machineLabel"  runat="server">43</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p126" CssClass="machineLabel"  runat="server">56</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p127" CssClass="machineLabel"  runat="server">57</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p140" CssClass="machineLabel"  runat="server">
                            140
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p72" CssClass="machineLabel"  runat="server">72</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p83" CssClass="machineLabel"  runat="server">13</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p86" CssClass="machineLabel"  runat="server">16</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p97" CssClass="machineLabel"  runat="server">27</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p100" CssClass="machineLabel"  runat="server">30</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p111" CssClass="machineLabel"  runat="server">41</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p114" CssClass="machineLabel"  runat="server">44</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p125" CssClass="machineLabel"  runat="server">55</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p128" CssClass="machineLabel"  runat="server">58</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p139" CssClass="machineLabel"  runat="server">
                            139
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p73" CssClass="machineLabel"  runat="server">73</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p82" CssClass="machineLabel"  runat="server">12</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p87" CssClass="machineLabel"  runat="server">17</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p96" CssClass="machineLabel"  runat="server">26</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p101" CssClass="machineLabel"  runat="server">31</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p110" CssClass="machineLabel"  runat="server">40</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p115" CssClass="machineLabel"  runat="server">45</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p124" CssClass="machineLabel"  runat="server">54</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p129" CssClass="machineLabel"  runat="server">59</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p138" CssClass="machineLabel"  runat="server">
                            138
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p74" CssClass="machineLabel"  runat="server">74</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p81" CssClass="machineLabel"  runat="server">11</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p88" CssClass="machineLabel"  runat="server">18</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p95" CssClass="machineLabel"  runat="server">25</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p102" CssClass="machineLabel"  runat="server">32</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p109" CssClass="machineLabel"  runat="server">39</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p116" CssClass="machineLabel"  runat="server">46</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p123" CssClass="machineLabel"  runat="server">53</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p130" CssClass="machineLabel"  runat="server">60</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p137" CssClass="machineLabel"  runat="server">
                            137
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p75" CssClass="machineLabel"  runat="server">75</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p80" CssClass="machineLabel"  runat="server">10</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p89" CssClass="machineLabel"  runat="server">19</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p94" CssClass="machineLabel"  runat="server">24</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p103" CssClass="machineLabel"  runat="server">33</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p108" CssClass="machineLabel"  runat="server">38</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p117" CssClass="machineLabel"  runat="server">47</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p122" CssClass="machineLabel"  runat="server">52</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p131" CssClass="machineLabel"  runat="server">61</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p136" CssClass="machineLabel"  runat="server">
                            136
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p76" CssClass="machineLabel"  runat="server">76</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p79" CssClass="machineLabel"  runat="server">9</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p90" CssClass="machineLabel"  runat="server">20</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p93" CssClass="machineLabel"  runat="server">23</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p104" CssClass="machineLabel"  runat="server">34</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p107" CssClass="machineLabel"  runat="server">37</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p118" CssClass="machineLabel"  runat="server">48</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p121" CssClass="machineLabel"  runat="server">51</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p132" CssClass="machineLabel"  runat="server">62</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p135" CssClass="machineLabel"  runat="server">
                            135
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p77" CssClass="machineLabel"  runat="server">77</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p78" CssClass="machineLabel"  runat="server">8</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p91" CssClass="machineLabel"  runat="server">21</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p92" CssClass="machineLabel"  runat="server">22</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p105" CssClass="machineLabel"  runat="server">35</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p106" CssClass="machineLabel"  runat="server">36</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p119" CssClass="machineLabel"  runat="server">49</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p120" CssClass="machineLabel"  runat="server">50</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p133" CssClass="machineLabel"  runat="server">63</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p134" CssClass="machineLabel"  runat="server">
                            134
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l6" CssClass="machineLabel2"  runat="server">6</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l7" CssClass="machineLabel2"  runat="server">7</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l8" CssClass="machineLabel2"  runat="server">8</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l9" CssClass="machineLabel2"  runat="server">9</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l10" CssClass="machineLabel2"  runat="server">10</asp:Label>
                    </asp:TableCell>
                             
          
                </asp:TableRow>
            </asp:Table>

                        <asp:Table ID="Table2" Style="width:100%" runat="server" >
               <asp:TableRow CssClass="titleRow" ID="titleRow3">
                    <asp:TableCell CssClass="titleLabelCell" columnspan="10" style="text-align:center">
                        <asp:Label ID="s3" CssClass="titleLabel" runat="server">

                            VA 14 / MM 13 (27) s3

                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
         
                 <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p141" CssClass="machineLabel"  runat="server">141</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p154" CssClass="machineLabel"  runat="server">14</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p155" CssClass="machineLabel"  runat="server">15</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p168" CssClass="machineLabel"  runat="server">28</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p169" CssClass="machineLabel"  runat="server">29</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p182" CssClass="machineLabel"  runat="server">42</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p183" CssClass="machineLabel"  runat="server">43</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p196" CssClass="machineLabel"  runat="server">56</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p197" CssClass="machineLabel"  runat="server">57</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p210" CssClass="machineLabel"  runat="server">
                            210
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p142" CssClass="machineLabel"  runat="server">142</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p153" CssClass="machineLabel"  runat="server">13</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p156" CssClass="machineLabel"  runat="server">16</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p167" CssClass="machineLabel"  runat="server">27</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p170" CssClass="machineLabel"  runat="server">30</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p181" CssClass="machineLabel"  runat="server">41</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p184" CssClass="machineLabel"  runat="server">44</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p195" CssClass="machineLabel"  runat="server">55</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p198" CssClass="machineLabel"  runat="server">58</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p209" CssClass="machineLabel"  runat="server">
                            209
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p143" CssClass="machineLabel"  runat="server">143</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p152" CssClass="machineLabel"  runat="server">12</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p157" CssClass="machineLabel"  runat="server">17</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p166" CssClass="machineLabel"  runat="server">26</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p171" CssClass="machineLabel"  runat="server">31</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p180" CssClass="machineLabel"  runat="server">40</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p185" CssClass="machineLabel"  runat="server">45</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p194" CssClass="machineLabel"  runat="server">54</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p199" CssClass="machineLabel"  runat="server">59</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p208" CssClass="machineLabel"  runat="server">
                            208
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p144" CssClass="machineLabel"  runat="server">144</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p151" CssClass="machineLabel"  runat="server">11</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p158" CssClass="machineLabel"  runat="server">18</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p165" CssClass="machineLabel"  runat="server">25</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p172" CssClass="machineLabel"  runat="server">32</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p179" CssClass="machineLabel"  runat="server">39</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p186" CssClass="machineLabel"  runat="server">46</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p193" CssClass="machineLabel"  runat="server">53</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p200" CssClass="machineLabel"  runat="server">60</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p207" CssClass="machineLabel"  runat="server">
                            207
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p145" CssClass="machineLabel"  runat="server">145</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p150" CssClass="machineLabel"  runat="server">10</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p159" CssClass="machineLabel"  runat="server">19</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p164" CssClass="machineLabel"  runat="server">24</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p173" CssClass="machineLabel"  runat="server">33</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p178" CssClass="machineLabel"  runat="server">38</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p187" CssClass="machineLabel"  runat="server">47</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p192" CssClass="machineLabel"  runat="server">52</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p201" CssClass="machineLabel"  runat="server">61</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p206" CssClass="machineLabel"  runat="server">
                            206
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p146" CssClass="machineLabel"  runat="server">146</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p149" CssClass="machineLabel"  runat="server">9</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p160" CssClass="machineLabel"  runat="server">20</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p163" CssClass="machineLabel"  runat="server">23</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p174" CssClass="machineLabel"  runat="server">34</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p177" CssClass="machineLabel"  runat="server">37</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p188" CssClass="machineLabel"  runat="server">48</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p191" CssClass="machineLabel"  runat="server">51</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p202" CssClass="machineLabel"  runat="server">62</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p205" CssClass="machineLabel"  runat="server">
                            205
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p147" CssClass="machineLabel"  runat="server">147</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p148" CssClass="machineLabel"  runat="server">8</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p161" CssClass="machineLabel"  runat="server">21</asp:Label>
                    </asp:TableCell>
                                       <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p162" CssClass="machineLabel"  runat="server">22</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p175" CssClass="machineLabel"  runat="server">35</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p176" CssClass="machineLabel"  runat="server">36</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p189" CssClass="machineLabel"  runat="server">49</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p190" CssClass="machineLabel"  runat="server">50</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p203" CssClass="machineLabel"  runat="server">63</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell">
                        <asp:Label ID="p204" CssClass="machineLabel"  runat="server">
                            204
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow CssClass="machineLine">
                    <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l11" CssClass="machineLabel2"  runat="server">11</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l12" CssClass="machineLabel2"  runat="server">12</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l13" CssClass="machineLabel2"  runat="server">13</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l14" CssClass="machineLabel2"  runat="server">14</asp:Label>
                    </asp:TableCell>
                                        <asp:TableCell CssClass="machineCell2" ColumnSpan="2">
                        <asp:Label ID="l15" CssClass="machineLabel2"  runat="server">15</asp:Label>
                    </asp:TableCell>
                             
          
                </asp:TableRow>
            </asp:Table>
                         
                         <br />
                         <br />
        </div>
                </div>
            </asp:Panel>
                <asp:Panel runat="server" CssClass="center" ID="panel_operatori">
                    <h4 style="color:red;text-align:center" id="op_error" runat="server"></h4>
                     <div class="container" id="" style="margin-top:20px;margin-bottom:20px;">
                        <div class="row">
                            <div class="col-xs-12"><asp:TextBox runat="server" CssClass="col-xs-6" ID="txt_linea" style="height:40px;width: 425px;" placeholder="click and scan your linea"  ></asp:TextBox>
                            </div>
                             </div>
                         </div>
                    <asp:Panel runat="server" ID="pnl_operator_machines" style="width: 425px;    direction: rtl;"></asp:Panel>
                    <asp:Panel runat="server" ID="pnl_operator_machines_second" style="width:425px;margin-left:5px; direction: ltr"></asp:Panel>
                </asp:Panel>
            <asp:Panel runat="server" ID="panel_alarm"> 
                <div id="pnlalarrma" runat="server"   align="center">
                    <div id="searchSection" align="center">
                 
                <asp:GridView ID="Grid_table" OnRowDeleting="GridView1_RowDeleting" AutoGenerateDeleteButton="true"  DataKeyNames="ID" CssClass="rgMasterTable" HeaderStyle-BackColor="#d0d3d7" runat="server" OnRowDataBound="table_RowDataBound" ClientIDMode="Static">
                   
                </asp:GridView>

            </div>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" id="panel_quality">
                <div id="pnlquality" runat="server"   >
                    <div class="container">
                        <br />
                    <table style="width: 100%;" class="tbl">
                        <tr class="row">
                            <td class="col-md-3 col-xs-4 tdb">Machina</td>
                            <td class="col-md-9 col-xs-8 tdb"><asp:Label runat="server" ID="lbl_machina"></asp:Label></td>
                        </tr>
                        <tr class="row">
                            <td class="col-md-3 col-xs-4 tdb">Commessa</td>
                            <td class="col-md-9 col-xs-8 tdb"><asp:TextBox runat="server" ID="txt_commessa"></asp:TextBox></td>
                        </tr>
                        <tr class="row">
                            <td class="col-md-3 col-xs-4 tdb">Articolo</td>
                            <td class="col-md-9 col-xs-8 tdb"><asp:TextBox runat="server" ID="lbl_articolo"></asp:TextBox></td>
                        </tr>
                        <tr class="row">
                            <td class="col-md-3 col-xs-4 tdb">Componente</td>
                            <td class="col-md-9 col-xs-8 tdb"><asp:TextBox runat="server" ID="lbl_componente"></asp:TextBox></td>
                        </tr>
                        <tr class="row">
                            <td class="col-md-3 col-xs-4 tdb">Taglia</td>
                            <td class="col-md-9 col-xs-8 tdb"><asp:TextBox runat="server" ID="lbl_taglia"></asp:TextBox></td>
                        </tr>
                        <tr class="row">
                            <td class="col-md-3 col-xs-4 tdb">Colore</td>
                            <td class="col-md-9 col-xs-8 tdb"><asp:TextBox runat="server" ID="txt_colore"></asp:TextBox></td>
                        </tr>
                        <tr class="row">
                            <td class="col-md-3 col-xs-4 tdb">Cotta</td>
                            <td class="col-md-9 col-xs-8 tdb"><asp:TextBox runat="server" ID="txt_cotta"></asp:TextBox></td>
                        </tr>
                        <tr class="row">
                            <td class="col-md-3 col-xs-4 tdb">Progressivo</td>
                            <td class="col-md-9 col-xs-8 tdb"><asp:Label runat="server" ID="lbl_progressivo"></asp:Label></td>
                        </tr> 

                        <tr class="row hdr">
                             <td class="col-md-3 col-xs-4 " style="font-weight: bold;">Dati Qualita</td>
                        </tr>
                        </table>

                        <div class="row">
                            <div class="col-md-2 name">
                                Elasticita
                            </div>
                            <div class="col-md-1">
                                <span style="min-width:100px;">Aghi </span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txt_aghi" Enabled="true" style="width: 80%;    margin-bottom: 5px;"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <span style="min-width:100px;">Ranghi </span>

                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server"  ID="txt_ranghi" Enabled="true" style="width: 80%;    margin-bottom: 5px;"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2 name">
                                Misure
                            </div>
                            <div class="col-md-1">
                                <span style="min-width:100px;">Lunghezza </span>
                            </div>
                            <div class="col-md-4">
                               <asp:TextBox runat="server" ID="txt_lunghezza" Enabled="true" style="width: 80%;    margin-bottom: 5px;"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <span style="min-width:100px;">Langhezza </span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server"  ID="txt_langhezza" Enabled="true" style="width: 80%;    margin-bottom: 5px;"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2 name">
                                 Bordo/Polso/Collo
                            </div>
                            <div class="col-md-1">
                                <span style="min-width:100px;">Elasticita</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txt_elasticita" Enabled="true" style="width: 80%;    margin-bottom: 5px;"></asp:TextBox>
                             </div>
                            <div class="col-md-1">
                                <span style="min-width:100px;">Altezza</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server"  ID="txt_altezza" Enabled="true" style="width: 80%;    margin-bottom: 5px;"></asp:TextBox>
                             </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2 name">
                                 Catanelle/Cimose
                            </div>
                            <div class="col-md-1">
                                <span style="min-width:100px;">Catanelle</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txt_catanelle" Enabled="true" style="width: 80%;    margin-bottom: 5px;"></asp:TextBox>
                             </div>
                            <div class="col-md-1">
                                <span style="min-width:100px;">Cimose</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server"  ID="txt_cimose" Enabled="true" style="width: 80%;    margin-bottom: 5px;"></asp:TextBox>
                             </div>
                           
                        </div>

                        <div class="row">
                            <div class="col-md-1 col-md-offset-7">
                                <span style="min-width:100px;">Sx</span>
                            </div>
                             <div class="col-md-4 ">
                                <asp:TextBox runat="server"  ID="txt_sx" Enabled="true" style="width: 80%;    margin-bottom: 5px;"></asp:TextBox>
                             </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2 name">
                                <span>Note Alarm</span>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txt_note" style="width:100%;    margin-bottom: 5px;"></asp:TextBox>
                            </div>
                        </div>

                          <div class="row">
                            <div class="col-md-2 name">
                                <span style="padding-right:20px;">Esito</span>
                                <asp:CheckBox runat="server" ID="cb_esito" OnCheckedChanged="cb_esito_CheckedChanged" AutoPostBack="true" />

                            </div>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" Enabled="false" ID="txt_esito" style="width:30%; margin-bottom: 5px;"></asp:TextBox>
                            </div>
                        </div>
                        <br />

                        <div class="col-md-3 col-xs-4 " style="font-weight: bold;">Controlli</div>
                        <hr style="background:#e0e2e3" />

                        <div class="row">
                            <div class="col-md-2 name">
                                <span>Cartellino/Scheda</span>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txt_cartellino" style="width:100%;    margin-bottom: 5px;"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2 name">
                                <span>Infilatura</span>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txt_infilatiura" style="width:100%;    margin-bottom: 5px;"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2 name">
                                <span>Cotta/Filo</span>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txt_cottafillato" style="width:100%;    margin-bottom: 5px;"></asp:TextBox>
                            </div>
                        </div>
                        <br /><br />
                        <div class="row" style="display:none;">
                            <div class="col-md-2 name">
                                <span>Add Images</span>
                            </div>
                            <div class="col-md-2">
                                            <asp:ImageButton runat="server" ID="button_image" ImageUrl="~/Images/addimage.png" OnClick="button_image_Click" />
                            </div>
                        </div>
                         <br /><br />
                        <div class="row">
                            <div class="col-md-4">
                                <%--<asp:TextBox runat="server" ID="txt_emailToStart" placeholder="To: somemail@olimpias.rs"></asp:TextBox>--%>
                                <asp:DropDownList runat="server" style="display:none;" ID="ddp_mail_list" AutoPostBack="true" OnTextChanged="ddp_mail_list_TextChanged">
                                </asp:DropDownList>
                                
                                <asp:FileUpload runat="server" style="float:left;margin-bottom:20px;"  ID="Attach_to_mail" AllowMultiple="true"   />
                                <br />
                                <asp:Button runat="server" ID="btn_emailToSend" Text="Send Email" OnClick="btn_emailToSend_Click" />
                            </div>
                        </div>
                        <br />
                        <br />
                     
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="panel_image" runat="server">
             <div class="container" style="width:100%;height:100%;position:fixed;z-index:9999;top:0;bottom:0;background:white;padding-left:50px;padding-right:50px;">
                
                 <div class="row">
                     <h3>Image Upload:</h3>
                 </div>
                 <div class="row">
                     <div class="col-md-12">
                          <asp:FileUpload runat="server" style="float:left;"  ID="File_image" AllowMultiple="true"   />
                         <br />
                         <asp:Label runat="server" ID="lbl_error_image"></asp:Label>
                     </div>
                 </div>
                 <br /><br />
                 <div class="row">
                      <div class="col-md-2 col-md-offset-8">
                            <asp:Button runat="server" ID="btn_closeImage" Text="Close" CssClass="btn btn-warning" style="margin-right:15px;" OnClick="btn_closeImage_Click"/>
                            </div>
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btn_sendImage" CssClass="btn btn-success" Text="Send" OnClick="btn_sendImage_Click" accept ="image/gif, image/jpeg, image/png"  /> 
                        </div>
                 </div>
             </div>
         </asp:Panel>
            <asp:Panel ID="panel_myjob" runat="server">
                <div class="container" align="center">
                    <div class="row">
                        <br />
                        <div class="col-md-12">
                            <asp:GridView runat="server" ID="gv_myJob" AutoGenerateColumns="true" CssClass="rgMasterTable" HeaderStyle-BackColor="#d0d3d7"></asp:GridView>
                           <%-- <asp:SqlDataSource runat="server" ID="sqlConn"
                                ConnectionString="<%$connectionStrings:Sinotico%>" 
                                UpdateCommand=""/>--%>
                        </div>
                    </div>
                </div>
            </asp:Panel>
           

            </ContentTemplate>
             <Triggers>
        <asp:PostBackTrigger ControlID = "btn_emailToSend" />
    </Triggers>
        </asp:UpdatePanel> 
         <asp:Panel runat="server" ID="panel_email" style="width:100%;height:100%;position:fixed;z-index:9999;top:0;bottom:0;background:white;">
                <div class="container" align="center" style="padding-top:100px;">
                <div class="row"> 
                        <div class="col-md-2">
                            <span>Email To:</span>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txt_mailTo" placeholder="Must be @olimpias.rs/.it" TextMode="Email" style="width:100%;"></asp:TextBox>
                        </div> 
                </div>
                    <br />
                     <div class="row"> 
                        <div class="col-md-2">
                            <span>Subject:</span>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txt_subject" style="width:100%;"></asp:TextBox>
                        </div> 
                </div>
                    <br />
                    <div class="row">
                        <div class="col-md-2">
                            <span>Message:</span>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txt_mailMessage" style="width:100%;height: 100px;"></asp:TextBox>
                            
                        </div>
                    </div>
                    <br />
                   

                    <div class="row">
                        <div class="col-md-2">
                            <span>Attachment:</span>
                        </div>
                        <div class="col-md-10" >
                          
                          <asp:FileUpload runat="server" style="float:left;"  ID="file_attach" AllowMultiple="true"   />
                        </div>
                    </div>
                    <br /><br />
                    <div class="row">
                        <div class="col-md-2 col-md-offset-8">
                            <asp:Button runat="server" ID="button_closeMail" Text="Close" CssClass="btn btn-warning" style="margin-right:15px;" OnClick="button_closeMail_Click"/>
                            </div>
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="button_sendMail" CssClass="btn btn-success" Text="Send" OnClick="button_sendMail_Click" accept ="image/gif, image/jpeg, image/png"  /> 
                        </div>
                    </div> 
                     
                </div>

            </asp:Panel>
        
         


        <script type="text/javascript">

            Sys.Application.add_load(function () {
                //NAVIGATOR START
                //each machineCell -> id -> append new span with same id

                $('.machineCell').find('span').each(function() {
                    var id = $(this).attr('id');
                    $(this).prepend('<span class="brmachine">' + id.substring(1) + '</span>');

                });

                $("#idhome").click(function() {
                    $("#button_home").click(); 
                });
                $("#id_layout_cq_eff").click(function () { 

                    $("#button_lay_eff").click();
       
                });
                $("#id_layout_cq_pulizia").click(function () { 
                    $("#button_lay_pul").click();
         
                });
                $("#id_layout_cq_cq").click(function () {
                    $("#button_lay_cq").click(); 
                });
                $("#id_layout_cq_alarms").click(function () {
                    $("#button_cq_alarms").click(); 
                }); 
                $("#id_layout_pul_pul").click(function () {
                    $("#button_lay_pul").click(); 
                });
                $("#id_layout_pul_alarms").click(function () {
                    $("#button_pul_alarms").click(); 
                });

                $("#id_myJob").click(function () {
                    $("#button_myJob").click();
                })
                

                if ($('#lblJobType').text() == 'CQ') {
                    $('#org').hide();
                    $('#id_layout_cq_eff').show();
                    $('#id_layout_cq_pulizia').show();
                    $('#id_layout_cq_cq').show();
                    $('#id_layout_cq_alarms').show();
                    $('#id_myJob').show();

                } else if ($('#lblJobType').text() == 'PULIZIA ORDINARIA') {
                    $('#org').show();
                    $('#id_layout_pul_pul').show();
                    $('#id_layout_pul_alarms').show();
 
                } else if ($('#lblJobType').text() == 'OPERATORI') {
                    $('#id_layout_pul_pul').hide();
                    $('#id_layout_pul_alarms').hide();
                    $('#org').hide();
                    $('#id_layout_cq_eff').hide();
                    $('#id_layout_cq_pulizia').hide();
                    $('#id_layout_cq_cq').hide();
                    $('#id_layout_cq_alarms').hide();
                    $('#id_myJob').hide();
                    $('#panel_home_labels').show();
                }
                else
                {
                    $('#org').hide();
                } 
                $("#aggiorna_eff").click(function () {
                    setTimeout(
                        function () {
                            $('#pnlhome').hide();
                            $('#pnlpulizia').show();
                            $('#pnlalarrma').hide();
                            $('#org').hide();
                            $('#idallarme').removeClass('active');
                            $('#idpulizia').addClass('active');
                            $('#idhome').removeClass('active');
                        },
                        500);
                });
                $("#aggiorna_pul").click(function () {
                    setTimeout(
                        function () {
                            $('#pnlhome').hide();
                            $('#pnlpulizia').show();
                            $('#pnlalarrma').hide();
                            $('#org').hide();
                            $('#idallarme').removeClass('active');
                            $('#idpulizia').addClass('active');
                            $('#idhome').removeClass('active');
                        },
                        500);
                });
                $("#aggiorna_cq").click(function () {
                    setTimeout(
                        function () {
                            $('#pnlhome').hide();
                            $('#pnlpulizia').show();
                            $('#pnlalarrma').hide();
                            $('#org').hide();
                            $('#idallarme').removeClass('active');
                            $('#idpulizia').addClass('active');
                            $('#idhome').removeClass('active');
                        },
                        500);
                }); 
            })

        </script>
    </form>
</body>
</html>
