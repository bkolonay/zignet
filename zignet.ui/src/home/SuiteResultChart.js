import React, { Component } from 'react';

class SuiteResultChart extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <p>{this.props.suiteName}</p>
    );
  }
}

export default SuiteResultChart;
