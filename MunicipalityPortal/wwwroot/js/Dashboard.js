
    //<script>
    //    google.charts.load('current', { 'packages': ['corechart'] });
    //    google.charts.setOnLoadCallback(drawCharts);


    //    function drawCharts() {

    //        drawPieChart(@Model.NoPerm54A56,@Model.NoFixedTerm54A56,@Model.NoPermNon54A56, @Model.NoFixedTermNon54A56, @Model.NoOther);


    //        drawChart(@Model.Level1Percent, @Model.Level2Percent, @Model.Level3Percent,@Model.Level4Percent, "chartOverall", "");

    //        @foreach(var modelCategory in @Model.MaturityLevelDashboardViewModel.Sections)
    //        {
    //            string chartName = "chart" + modelCategory.PageNo;
    //            var categoryLevels = modelCategory.QuestionCategoryLevels;
    //             @:drawChart(@categoryLevels.Level1Percentage, @categoryLevels.Level2Percentage, @categoryLevels.Level3Percentage,@categoryLevels.Level4Percentage,"@chartName","@modelCategory.CategoryName")
    //        }
    //    }


    //    function drawPieChart(permSect5657,fixedTerm5657,permNonSect5657,fixedTermNon5657,others) {

    //        var data = google.visualization.arrayToDataTable([
    //            ['Category', '% of employees'],
    //            ['Permanent sect. 56 & 57', permSect5657],
    //            ['Fixed-term  56 & 57', fixedTerm5657],
    //            ['Permanent  non-sect 56 & 57', permNonSect5657],
    //            ['Fixed-term non-sect 56 & 57', fixedTermNon5657],
    //            ['others not on payroll', others]
    //        ]);
    //        var options = {
    //            pieSliceText: "value",
    //            legend: "none",
    //            is3D: true,
    //            backgroundColor: "transparent",
    //            chartArea: { width: "100%", height: "65%" },
    //            legend: {
    //                position: 'labeled',
    //                labeledValueText: 'both',
    //                textStyle: {
    //                    color: 'blue',
    //                    fontSize: 14
    //                }
    //            }
    //        };

    //        var chart = new google.visualization.PieChart(document.getElementById('piechart'));

    //        chart.draw(data, options);
    //    }

    //    function drawChart(level1Val, level2Val, level3Val, Level4Val, chartID, chartTitle) {
    //        var data = google.visualization.arrayToDataTable([
    //            ["Percent", "Level", { role: "style" }, { role: 'annotation' } ],
    //            ["Level 1", level1Val / 100, "red", level1Val],
    //            ["Level 2", level2Val / 100, "orange", level2Val],
    //            ["Level 3", level3Val / 100, "yellow", level3Val],
    //            ["Level 4", Level4Val / 100, "LawnGreen", Level4Val]
    //        ]);
    //        var view = new google.visualization.DataView(data);


    //        var options = {
    //            chartArea: {
    //                // leave room for y-axis labels
    //                width: '80%'
    //            },
    //            legend: {
    //                position: 'top'
    //            },
    //            title: chartTitle,
    //            titleTextStyle: {
    //                color: "white"
    //            },
    //            bar: { groupWidth: "70%" },
    //            legend: { position: "none" },
    //            backgroundColor: {
    //                fill: "black",
    //                stroke: '#green',
    //                strokeWidth: 3
    //            },
    //            vAxis: {
    //                format: "#.#%",
    //                maxValue: 100,
    //                viewWindow: { max: 1 },
    //                textStyle: {
    //                    color: "white"
    //                }
    //            },
    //            hAxis: {
    //                textStyle: {
    //                    color: "white"
    //                }
    //            },
    //            tooltip: {
    //                trigger: "none"
    //            }
    //        };
    //        var chart = new google.visualization.ColumnChart(document.getElementById(chartID));
    //        chart.draw(view, options);
    //    }

    //</script>