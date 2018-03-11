import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import ChartistPieChart from '../common/ChartistPieChart';

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

  render() {
    const suiteResult = this.props.suiteResult;
    return (
      <div className="col-4">
        <h3 className="text-center">{suiteResult.SuiteName}</h3>
        <ChartistPieChart chartId={suiteResult.SuiteID}
                          chartData={this._getChartData(suiteResult)} />
        <p className="text-center"><Link to={`/suiteResult/${suiteResult.SuiteID}`}>Total: {this._getTotalTests(suiteResult)}</Link></p>
      </div>
    );
  }
}

export default SuiteResultChart;
