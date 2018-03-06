import React, { Component } from 'react';

import { Link } from 'react-router-dom'

import ChartistPieChart from '../common/ChartistPieChart';

class SuiteResultChart extends Component {

  _getChartData() {
    return {
      series: [
        this.props.suiteResult.TotalPassedTests,
        this.props.suiteResult.TotalFailedTests
      ]
    };
  }

  _getTotalTests() {
    return this.props.suiteResult.TotalPassedTests + this.props.suiteResult.TotalFailedTests;
  }

  render() {
    return (
      <div className="col-4">
        <h3 className="text-center">{this.props.suiteResult.SuiteName}</h3>
        <ChartistPieChart chartId={this.props.suiteResult.SuiteID}
                          chartData={this._getChartData()} />
        <p className="text-center"><Link to="/suiteResult">Total: {this._getTotalTests()}</Link></p>
      </div>
    );
  }
}

export default SuiteResultChart;
