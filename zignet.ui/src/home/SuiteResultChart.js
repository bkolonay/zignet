import React, { Component } from 'react';

import ChartistPieChart from '../common/ChartistPieChart';

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

  render() {
    return (
      <div>
        <h2>{this.suiteResult.suiteName}</h2>
        <ChartistPieChart chartId={this.suiteResult.suiteResultId}
                          chartData={this.chartData} />
      </div>
    );
  }
}

export default SuiteResultChart;
