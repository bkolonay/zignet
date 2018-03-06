import React, { Component } from 'react';

class SuiteResult extends Component {

  render() {
    return (
      <p>Suite result page: {this.props.match.params.suiteResultId}</p>
    );
  }
}

export default SuiteResult;
