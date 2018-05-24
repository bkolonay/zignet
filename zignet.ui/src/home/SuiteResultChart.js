import React, { Component } from 'react';
import ChartistPieChart from '../common/chartist/ChartistPieChart';
import ListPageLink from './ListPageLink';
import UtcDate from '../common/UtcDate'
import './css/suiteResultChart.css';

class SuiteResultChart extends Component {

  _getChartData(suiteResult) {
    return {
      series: [
        suiteResult.TotalPassedTests,
        suiteResult.TotalFailedTests
      ]
    };
  }

  _getTotalTests(suiteResult) {
    return suiteResult.TotalPassedTests + suiteResult.TotalFailedTests;
  }

  _getLastRunTime(suiteResult, grouped) {
    if (grouped)
      return <p/>;
    if (suiteResult.SuiteEndTime)
      return <p className="text-center text-muted"><small>{new UtcDate(suiteResult.SuiteEndTime).getTimeFromNowWithSuffix()}</small></p>;
    else
      return <p className="text-center text-warning"><small>running...</small></p>;
  }

  render() {
    const suiteResult = this.props.suiteResult;
    const suiteId = suiteResult.SuiteIds[0];
    const grouped = this.props.grouped;
    const totalTests = suiteResult.TotalPassedTests + suiteResult.TotalFailedTests;
    return (
      <div className="col-4">
        <h3 className="text-center">{suiteResult.SuiteName}</h3>
        <ChartistPieChart chartId={suiteResult.SuiteIds[0]} chartData={this._getChartData(suiteResult)} />
        <p className="text-center chart-label">
          <ListPageLink grouped={grouped} suiteId={suiteId} totalTests={totalTests} />
        </p>
        {this._getLastRunTime(suiteResult, grouped)}
      </div>
    );
  }
}

export default SuiteResultChart;
