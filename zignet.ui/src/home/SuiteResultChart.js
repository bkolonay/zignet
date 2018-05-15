import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import ChartistPieChart from '../common/ChartistPieChart';
import UtcDate from '../common/UtcDate'
import './suiteResultChart.css';

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

  _getLastRunTime(suiteResult) {
    if (suiteResult.SuiteEndTime)
      return <p className="text-center text-muted"><small>{new UtcDate(suiteResult.SuiteEndTime).getTimeFromNowWithSuffix()}</small></p>
    else
      return <p className="text-center text-warning"><small>running...</small></p>
  }

  _getLinkToListPage(suiteResult, suiteResultsGrouped) {
    if (suiteResultsGrouped)
      return <Link to={'/' + suiteResult.SuiteIds[0] + "?group=true"}>Total: {this._getTotalTests(suiteResult)}</Link>
    else
      return <Link to={'/' + suiteResult.SuiteIds[0]}>Total: {this._getTotalTests(suiteResult)}</Link>
  }

  render() {
    const suiteResult = this.props.suiteResult;
    return (
      <div className="col-4">
        <h3 className="text-center">{suiteResult.SuiteName}</h3>
        <ChartistPieChart chartId={suiteResult.SuiteIds[0]}
                          chartData={this._getChartData(suiteResult)} />
        <p className="text-center chart-label">
          {this._getLinkToListPage(suiteResult, this.props.suiteResultsGrouped)}
        </p>
        {this._getLastRunTime(suiteResult)}
      </div>
    );
  }
}

export default SuiteResultChart;
