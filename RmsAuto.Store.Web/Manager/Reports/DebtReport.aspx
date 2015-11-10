<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true"  %>
<%@ Register src="~/Manager/Controls/SupplyReportTable.ascx" tagname="SupplyReportTable" tagprefix="uc3" %>
<%@ Register src="~/Manager/Controls/ClientSearchList.ascx" tagname="ClientSearchList" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/ext-all.js") %>" type="text/javascript"></script>  
    <script src="<%= ResolveUrl("~/Scripts/src/grid/groupingStore.js") %>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/knockout-3.1.0.js") %>" type="text/javascript"></script>

    <style type="text/css" >
        .x-grid-row-summary {
            background-color: #CCCCCC;
            color:Fuchsia;
            font-weight:bolder !important;
            font-size:large !important;
        }
    </style>

    <script type="text/javascript">

        Ext.Loader.setConfig({ enabled: true });

        Ext.require([
    'Ext.grid.*',
    'Ext.data.*',
    'Ext.form.*',
    'Ext.Msg.*'
]);

        //Модель данных
        Ext.define('Task', {
            extend: 'Ext.data.Model',
            idProperty: 'num',
            fields: [
        { name: 'ClientName', type: 'string' },
        { name: 'DaysDelayed', type: 'string' },
        { name: 'DocumentSum', type: 'int' },
        { name: 'DateOfBegin', type: 'string', dateFormat: 'm/d/Y' },
        { name: 'DebtSumm', type: 'float' },
        { name: 'DelayedSumm', type: 'float' },
        { name: 'DocSumm', type: 'float' },
        { name: 'PaymentSum', type: 'float' }
    ]
    });

    //Модель данных
    Ext.define('Client', {
        extend: 'Ext.data.Model',
        idProperty: 'ClientID',
        fields: [
        { name: 'ClientName', type: 'string' }

    ]
    });

    Ext.onReady(function() {
    
    var ResultViewModel = {
        ClientName: ko.observable(""),
        DateOfBegin: ko.observable(""),
        DaysDelayed:  ko.observable(""),
        DebtSumm: ko.observable(""),
        DelayedSumm: ko.observable(""),
        PaymentSum: ko.observable("")
    }
    
    var i = 0;

        var dateMenu = Ext.create('Ext.menu.DatePicker', {
        //                handler: function(dp, date) {
        //                    Ext.Msg.alert('Date Selected', 'You choose {0}.', Ext.Date.format(date, 'M j, Y'));
        //                }
    });

    Ext.Date.patterns = {
        ISO8601Long: "d-m-Y H:i:sP",
        ISO8601Short: "d-m-Y",
        ShortDate: "j/n/y",
        FullDateTime: "l, F d, Y g:i:s A",
        LongTime: "g:i:s A",
        SortableDateTime: "d-m-Y\\ H:i:s",
        UniversalSortableDateTime: "Y-m-d H:i:sO",
        CustomFormat: "H:i d-m"
    };

    var store = Ext.create('Ext.data.GroupingStore', {
        //Здесь указываем название ранее созданной модели
        model: 'Task',
        pageSize: 5,
        autoLoad: true,
        remoteSort: true,
        remoteGroup: true,
        proxy: {
            type: 'rest',
            url: 'dataHandlers/Debts.ashx',
            reader: {
                type: 'json',
                root: 'items',
                id: 'num',
                totalProperty: 'totalCount'
            }
        },
        sorters: { property: 'ClientName', direction: 'ASC' },
        groupField: 'ClientName'
    });

    var ClientStore = Ext.create('Ext.data.Store', {
        //Здесь указываем название ранее созданной модели
        model: 'Client',
        autoLoad: true,
        remoteSort: true,
        remoteGroup: true,
        proxy: {
            type: 'ajax',
            url: 'dataHandlers/Clients.ashx',
            reader: {
                type: 'json',
                root: ''
            }
        },
        sorters: { property: 'ClientName', direction: 'ASC' },
        groupField: 'ClientName'
    });



    var grid = Ext.create('Ext.grid.Panel', {
        width: 1600,
        height: 800,
        //    plugins: summary,
        //        view: new Ext.grid.GroupingView({
        //            forceFit: true,
        //            showGroupName: true,
        //            enableNoGroups: false, // Required
        //            hideGroupedColumn: true
        //        }),
        title: 'Debt receivable report',
        renderTo: 'test',
        store: store,
        stateful: true,
        viewConfig: {
            id: 'gv',
            trackOver: false,
            stripeRows: false,
            plugins: [{
                ptype: 'preview',
                bodyField: 'excerpt',
                expanded: true,
                pluginId: 'preview'
}]
            },
        bbar: new Ext.PagingToolbar({
            doRefresh: function() {
                alert('');
            },
            store: store,
            displayInfo: true,
            displayMsg: '{0} - {1} из {2}',
            emptyMsg: "Nothing is found"
        }),
        viewConfig: {
            stripeRows: false
        },
        dockedItems: [{
            dock: 'top',
            xtype: 'toolbar',
            items: [{
                xtype: 'datefield',
                anchor: '50%',
                fieldLabel: 'On date:',
                name: 'to_date',
                menu: dateMenu,
                value: new Date(),  // defaults to today
                listeners: {
                    change: function(combo, value) {
                        $.ajax({
                            url: 'dataHandlers/PutSession.ashx?ParamName=dateEnd&ParamValue=' + Ext.Date.format(value, Ext.Date.patterns.ISO8601Short),
                            type: 'POST'

                        });
                    }
                }
            }
                        , {
                            xtype: 'combo',
                            fieldLabel: 'Counterparty:',
                            hiddenName: 'ClientStore',
                            width: 400,
                            store: ClientStore,
                            valueField: 'ClientID',
                            displayField: 'ClientName',
                            triggerAction: 'all',
                            editable: true,
                            listeners: {
                                change: function(combo, value) {
                                    $.ajax({
                                        url: 'dataHandlers/PutSession.ashx?ParamName=DateReportClientName&ParamValue=' + value,
                                        type: 'POST'
                                    });
                                }
                            }
                        },
                        {
                            text: 'Form',
                            handler: function(btn, pressed) {

                                var observableData;
                                
                                grid.store.reload();
                                $.ajax({
                                       url: 'dataHandlers/SummaryString.ashx',
                                       type: 'POST',
                                       dataType: 'json',
                                       success: function(data)
                                       {
                                            if (i == 0) //Биндинги применяем только один раз
                                            {
                                                i++;
                                                ko.applyBindings(ResultViewModel);            
                                            }
                                            //$('<table><tr><td data-bind="text: ClientName"></td><td data-bind="text: DateOfBegin"></td></tr></table>').insertAfter('.x-panel');
                                           // ko.cleanNode($('#summaryString'));
                                            ResultViewModel.ClientName(data.root[0].ClientName);  
                                            ResultViewModel.DaysDelayed(data.root[0].DaysDelayed);  
                                            ResultViewModel.DateOfBegin(data.root[0].DateOfBegin);  
                                            ResultViewModel.DelayedSumm(data.root[0].DelayedSumm);
                                            ResultViewModel.DebtSumm(data.root[0].DebtSumm);
                                            ResultViewModel.PaymentSum(data.root[0].PaymentSum);
                                       }
                                });
                             }
                        },
                        {
                            text: 'Display summary',
                            pressed: true,
                            enableToggle: true,
                            toggleHandler: function(btn, pressed) {



                                var view = grid.getView();
                                gFeature = view.features[0]; //I j&uacute;st have one feautre... so [0] works
                                gFeature.toggleSummaryRow(pressed);
                                view.refresh();
                            }
                        }
                            ,
                            {
                                text: 'Expand all',
                                pressed: false,
                                enableToggle: true,
                                toggleHandler: function(btn, pressed) {
                                    var view = grid.getView();
                                    gFeature = view.features[0]; //I j&uacute;st have one feautre... so [0] works
                                    if (pressed) {
                                        gFeature.collapseAll();
                                        btn.text = 'Collapse all';
                                    }
                                    else {
                                        gFeature.expandAll();
                                        btn.text = 'Expand all';
                                    }
                                    view.refresh();
                                }

}]
}],
            features: [{
                id: 'group',
                ftype: 'groupingsummary',
                groupHeaderTpl: '{name}',
                hideGroupedHeader: false,
                remoteRoot: 'summaryData',
                startCollapsed: false,
                enableGroupingMenu: false
            }, {
                ftype: 'summary',
                fixed: true, //if this is set to true, my fixed summary row will replace the built in summary row
                dock: 'bottom',
                remoteRoot: 'summaryData',
}],
                columns: [{
                    text: 'Клиент',
                    flex: 1,
                    id: 'ClientName',
                    sortable: true,
                    tdCls: 'ClientName',
                    dataIndex: 'ClientName',
                    hideable: false,
                    summaryType: 'count',
                    summaryRenderer: function(value, summaryData, dataIndex) {
                        return 'Всего ' + value;
                    }
                }, {
                    header: 'Просрочено дней',
                    width: 180,
                    sortable: true,
                    dataIndex: 'DaysDelayed',
                    summaryType: 'max',
                    renderer: function(value, metaData, record, rowIdx, colIdx, store, view) {
                        return value;
                    },
                    summaryRenderer: function(value, summaryData, dataIndex) {
                        return value;
                    }
                }, {
                    header: 'Сумма по документу',
                    width: 180,
                    sortable: true,
                    dataIndex: 'PaymentSum',
                    renderer: Ext.util.Format.currency("0 000.00 р."),
                    summaryType: 'max',
                    summaryRenderer: function(value, summaryData, dataIndex) {
                        return value;
                    }
                }, {
                    header: 'Дата возник-я задолженности',
                    width: 180,
                    /*xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('m/d/Y'),
                    summaryRenderer: Ext.util.Format.dateRenderer('m/d/Y'),*/
                    summaryType: 'min',
                    sortable: true,
                    dataIndex: 'DateOfBegin'

                }, {
                    header: 'Задолженность',
                    width: 180,
                    sortable: true,
                    renderer: Ext.util.Format.usMoney,

                    // summaryRenderer: Ext.util.Format. ,
                    renderer: Ext.util.Format.currency("0 000.00 р."),
                    dataIndex: 'DocSumm',
                    summaryType: 'sum',
                    summaryRenderer: function(value, summaryData, dataIndex) {
                        return value;
                    }
                }, {
                    id: 'DelayedSumm',
                    header: 'Просроченная задолженность',
                    width: 220,
                    sortable: false,
                    groupable: false,
                    renderer: Ext.util.Format.currency("0 000.00 р."),
                    dataIndex: 'DelayedSumm',
                    summaryType: 'sum',
                    summaryRenderer: function(value, summaryData, dataIndex) {
                        return value;
                    }
}]
                });
            });
       
   </script>
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <div style="padding: 10px" id="Div1">
        
        <div id="test">
        </div>
        
        <div style="padding: 2px" id="Div2">
        <table id="summaryString" cellpadding="0" cellspacing="0" class="x-table-plain x-grid-table">
        <tbody>
        <tr role="row" id="gridview-1031-record-gridview-1031-summary-record" data-boundview="gridview-1031" data-recordid="gridview-1031-summary-record" data-recordindex="-1" class=" x-grid-row-summary" tabindex="-1">
        <td role="gridcell" class="x-grid-cell x-grid-td x-grid-cell-ClientName ClientName x-grid-cell-first x-unselectable ClientName" id="ext-gen1206" style="width: 658px;">
            <div unselectable="on" class="x-grid-cell-inner " style="text-align:left;"  data-bind="text: ClientName"></div>
        </td>
        <td role="gridcell" class="x-grid-cell x-grid-td x-grid-cell-gridcolumn-1027 x-unselectable" id="ext-gen1207" style="width: 180px;">
            <div unselectable="on" class="x-grid-cell-inner " style="text-align:left;" data-bind="text: DaysDelayed" >4</div>
        </td>
        <td role="gridcell" class="x-grid-cell x-grid-td x-grid-cell-gridcolumn-1028 x-unselectable " id="ext-gen1208" style="width: 180px;">
            <div unselectable="on" class="x-grid-cell-inner " style="text-align:left;" data-bind="text: PaymentSum" ></div></td>
        <td role="gridcell" class="x-grid-cell x-grid-td x-grid-cell-gridcolumn-1029 x-unselectable " id="ext-gen1209" style="width: 180px;">
            <div unselectable="on" class="x-grid-cell-inner " style="text-align:left;" data-bind="text: DateOfBegin" ></div></td>
        <td role="gridcell" class="x-grid-cell x-grid-td x-grid-cell-gridcolumn-1030 x-unselectable " id="ext-gen1210" style="width: 180px;">
            <div unselectable="on" class="x-grid-cell-inner " style="text-align:left;" data-bind="text: DebtSumm"></div></td>
        <td role="gridcell" class="x-grid-cell x-grid-td x-grid-cell-DelayedSumm x-grid-cell-last x-unselectable" id="ext-gen1211" style="width: 220px;">
            <div unselectable="on" class="x-grid-cell-inner " style="text-align:left;" data-bind="text: DelayedSumm"></div>
         </td></tr></tbody></table>
         </div>
      
      </div>
</asp:Content>

 
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">

</asp:Content>

