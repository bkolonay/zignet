import React, { Component } from 'react';

class LatestSuiteResultsForSuite extends Component {

  render() {
    return (
      <p>Suite id: {this.props.match.params.suiteId}</p>
    );
  }
}

export default LatestSuiteResultsForSuite;
