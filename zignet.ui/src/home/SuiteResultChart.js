import React, { Component } from 'react';

import ChartistPieChart from '../common/ChartistPieChart';
import '../common/bootstrap.css'

class SuiteResultChart extends Component {
  constructor(props) {
    super(props);
    this.suiteResult = this.props.suiteResult;
    this.chartData = {
      series: [
        this.suiteResult.totalPassedTests,
        this.suiteResult.totalFailedTests
      ]
    }
  }

  getTotalTests() {
    return this.suiteResult.totalPassedTests + this.suiteResult.totalFailedTests;
  }

  render() {
    return (
      <div className="col-4">
        <h3 className="text-center">{this.suiteResult.suiteName}</h3>
        <ChartistPieChart chartId={this.suiteResult.suiteResultId}
                          chartData={this.chartData} />
        <p className="text-center">Total: {this.getTotalTests()}</p>
      </div>
    );
  }
}

export default SuiteResultChart;
