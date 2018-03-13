import React, { Component } from 'react';

class SuiteResults extends Component {

  render() {
    const testResult = this.props.testResult;
    return (
  	  <tr>
        <td>{testResult.TestName}</td>
        <td>{testResult.FailingFromDate}</td>
        <td></td>
      </tr>
    );
  }
}

export default SuiteResults;
