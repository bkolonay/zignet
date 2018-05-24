import React, { Component } from 'react';
import ChartistPieChart from '../common/chartist/ChartistPieChart';
import ListPageLink from './ListPageLink';
import LastRunTimeLabel from './LastRunTimeLabel';
import './css/suiteResultChart.css';

class SuiteResultChart extends Component {

  render() {

    const suiteResult = this.props.suiteResult;
    const suiteId = suiteResult.SuiteIds[0];
    const grouped = this.props.grouped;
    const totalTests = suiteResult.TotalPassedTests + suiteResult.TotalFailedTests;
    const chartData = {
      series: [
        suiteResult.TotalPassedTests,
        suiteResult.TotalFailedTests
      ]
    };

    return (
      <div className="col-4">
        <h3 className="text-center">{suiteResult.SuiteName}</h3>
        <ChartistPieChart chartId={suiteId} chartData={chartData} />
        <p className="text-center chart-label">
          <ListPageLink grouped={grouped} suiteId={suiteId} totalTests={totalTests} />
        </p>
        <LastRunTimeLabel grouped={grouped} suiteEndTime={suiteResult.SuiteEndTime} />
      </div>
    );
  }
}

export default SuiteResultChart;
