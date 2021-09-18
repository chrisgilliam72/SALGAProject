
window.drawChart = (level1Val, level2Val, level3Val, Level4Val, chartID, chartTitle) => {
    google.charts.load('current', { 'packages': ['corechart'] });
    google.setOnLoadCallback(drawChart);

    function drawChart() {
        var data = google.visualization.arrayToDataTable([
            ["Percent", "Level", { role: "style" }, { role: 'annotation' }],
            ["Level 1", level1Val / 100, "red", level1Val],
            ["Level 2", level2Val / 100, "orange", level2Val],
            ["Level 3", level3Val / 100, "yellow", level3Val],
            ["Level 4", Level4Val / 100, "LawnGreen", Level4Val]
        ]);
        var view = new google.visualization.DataView(data);


        var options = {
            chartArea: {
                // leave room for y-axis labels
                width: '80%'
            },
            legend: {
                position: 'top'
            },
            title: chartTitle,
            titleTextStyle: {
                color: "black"
            },
            bar: { groupWidth: "70%" },
            legend: { position: "none" },
            backgroundColor: {
                fill: "white",
                stroke: 'none',
                strokeWidth: 0
            },
            vAxis: {
                format: "#.#%",
                maxValue: 100,
                viewWindow: {min:0, max: 1 },
                textStyle: {
                    color: "black"
                }
            },
            hAxis: {
                textStyle: {
                    color: "black"
                }
            },
            tooltip: {
                trigger: "none"
            }
        };
        var chart = new google.visualization.ColumnChart(document.getElementById(chartID));
        chart.draw(view, options);
    }

};

window.showBusyModel = () => {
    $('#idShowBusy').modal('show')
}

window.hideBusyModel = () => {
    $('#idShowBusy').modal('hide')
}


window.showMunicipalityModal = () => {
    $('#idShowMunicipalityLst').modal('show')
}

window.hideMunicipalityModal = () => {
    $('#idShowMunicipalityLst').modal('hide')
}

window.onresize = ()=>
{
    google.charts.load('current', { 'packages': ['corechart'] });
    google.setOnLoadCallback(drawChart);

    function drawChart() {
        var data = google.visualization.arrayToDataTable([
            ["Percent", "Level", { role: "style" }, { role: 'annotation' }],
            ["Level 1", level1Val / 100, "red", 0],
            ["Level 2", level2Val / 100, "orange", 0],
            ["Level 3", level3Val / 100, "yellow", 0],
            ["Level 4", Level4Val / 100, "LawnGreen", 0]
        ]);
        var view = new google.visualization.DataView(data);


        var options = {
            chartArea: {
                // leave room for y-axis labels
                width: '80%'
            },
            legend: {
                position: 'top'
            },
            title: chartTitle,
            titleTextStyle: {
                color: "black"
            },
            bar: { groupWidth: "70%" },
            legend: { position: "none" },
            backgroundColor: {
                fill: "white",
                stroke: 'none',
                strokeWidth: 0
            },
            vAxis: {
                format: "#.#%",
                maxValue: 100,
                viewWindow: { min: 0, max: 1 },
                textStyle: {
                    color: "black"
                }
            },
            hAxis: {
                textStyle: {
                    color: "black"
                }
            },
            tooltip: {
                trigger: "none"
            }
        };
        var chart = new google.visualization.ColumnChart(document.getElementById("chartOverall"));
        chart.draw(view, options);
    }
}








