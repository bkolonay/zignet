import React, { Component } from 'react';

import ChartistPieChart from '../common/ChartistPieChart';
import '../common/bootstrap.css'

class SuiteResultChart extends Component {
  constructor(props) {
    super(props);
    this.suiteResult = this.props.suiteResult;
    this.chartData = {
      series: [
        this.suiteResult.TotalPassedTests,
        this.suiteResult.TotalFailedTests
      ]
    }
  }

  getTotalTests() {
    return this.suiteResult.TotalPassedTests + this.suiteResult.TotalFailedTests;
  }

  render() {
    return (
      <div className="col-4">
        <h3 className="text-center">{this.suiteResult.SuiteName}</h3>
        <ChartistPieChart chartId={this.suiteResult.SuiteID}
                          chartData={this.chartData} />
        <p className="text-center">Total: {this.getTotalTests()}</p>
      </div>
    );
  }
}

export default SuiteResultChart;
