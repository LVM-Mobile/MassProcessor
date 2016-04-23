<%@ Page Language="C#" MasterPageFile="~/TableProcessorWebApp.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TableProcessorWebApp.WebFormInputFile" Title="Select input data table" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="4" 
        onactivestepchanged="Wizard1_ActiveStepChanged" 
        onfinishbuttonclick="Wizard1_FinishButtonClick" Height="200px" 
        width="400px" DisplaySideBar="False">
        <FinishCompleteButtonStyle CssClass="longoperation" />
        <HeaderStyle BackColor="#F0F0F0" />
        <StepNextButtonStyle CssClass="PrevNextOperation" />
        
    <WizardSteps>
        <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1">
         <div class="header">Input data table file(s)</div>
          File: <input runat="server" ID="File1" type="file" style="width: 554px"></input>
         <br />
         <asp:CheckBox ID="CheckBoxEditMode" Label="Edit mode" runat="server" 
                OnCheckedChanged="CheckBoxEditMode_CheckedChanged" Checked="True" />Edit mode
        </asp:WizardStep>
        <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
            <div class="header">Select tables (data sheets) to process:</div>
            <asp:CheckBoxList ID="CheckBoxListTables" runat="server" Height="107px" Width="90%" ></asp:CheckBoxList>
         </asp:WizardStep>
        <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
           <div class="header">Select Processor module(s):</div>
           <asp:CheckBoxList ID="CheckBoxListModules" runat="server" Height="107px" Width="90%" ></asp:CheckBoxList>
        </asp:WizardStep>
        <asp:WizardStep ID="WizardStep4" runat="server" Title="Step 4">
            <div class="header">Map fields:</div>
            <table width="90%">
             <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
              <ItemTemplate>
               <tr>
                <td>
                 <asp:Label ID="Label1" runat="server" Text='<%# ((System.Data.DataRowView)Container.DataItem)["targetField"] %>'></asp:Label>
                </td>   
                <td>
                  <asp:DropDownList ID="DropDownList2" DataTextField="inputField" DataValueField="inputField" Runat="server" SelectedValue='<%# ((System.Data.DataRowView)Container.DataItem)["inputField"] %>' >
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>CA</asp:ListItem>
                            <asp:ListItem>VT</asp:ListItem>
                            <asp:ListItem>UT</asp:ListItem>
                            <asp:ListItem>MD</asp:ListItem>
                            </asp:DropDownList>            
                </td>
             </tr>
            </ItemTemplate>            
            </asp:Repeater>
          </table>
         </asp:WizardStep>
                <asp:WizardStep ID="WizardStep5" runat="server" Title="Step 5">
                <div class="header">Select output file: </div>
                    <asp:TextBox CssClass="OutputFileName"  ClientIDMode="Static"  
                        ID="TextBoxOutputFileName" runat="server" Width="90%" ReadOnly="True"></asp:TextBox>            
                    
                    <asp:HyperLink ID="DownloadUrl" runat="server" ClientIDMode="Static" NavigateUrl="#" 
                        >Download</asp:HyperLink>
                    <asp:HiddenField ID="SessionID" runat="server" 
                        OnValueChanged="HiddenField1_ValueChanged" ClientIDMode="Static" />
        </asp:WizardStep>
    </WizardSteps>
</asp:Wizard>

<div id="progressbar"><div id="progress_text" style="position:relative; left:0pt; top:0pt;"></div></div> 


</asp:Content>

